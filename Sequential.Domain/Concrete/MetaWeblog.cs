using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CookComputing.XmlRpc;
using Sequential2013.Domain.Abstract;
using Sequential2013.Domain.Models;
using System.Configuration;
using System.Security;
using System.Web.Security;

// Initial implementation taken from Scott Hanselman's NotABlog code sample from:
// http://www.hanselman.com/blog/TheWeeklySourceCode55NotABlogALocalXMLRPCMetaWebLogEndpointThatLiesToWindowsLiveWriter.aspx

//50% of this code came from Keyvan Nayyeri's excellent blog post. 
// http://nayyeri.net/implement-metaweblog-api-in-asp-net. He rocks. Nice job.
// The XmlRpcService is the timeless work of Charles Cook. Also rocks and has for a long time.
// I mashed it up.
// Scott Hanselman - http://hanselman.com
namespace Sequential2013.Domain.Concrete {
   public class MetaWeblog : XmlRpcService, IMetaWeblog {

      private string SeqDBCon;
      private ISeqPostsRepository postsRep;
      private ISeqCategoriesRepository catRep;
      private ISeqTagsRepository tagRep;
		private const int INVALID_CREDENTIALS = 100;
		private const string INVALID_CREDENTIALS_MSG =
			"Authentication failure. Please check your credentials.";
		private const int METHOD_NOT_ACCESSIBLE = 200;
		private const string METHOD_NOT_ACCESSIBLE_MSG =
			"This method is not accessible remotely.";
		private const int MEDIAOBJECT_IO_FAILURE = 301;
		private const int MEDIAOBJECT_PERMISSION_FAILURE = 302;

      #region Public Constructors
      public MetaWeblog(string connectionString) {
         SeqDBCon = connectionString;
         postsRep = new SeqPostsRepository(SeqDBCon);
			catRep = new SeqCategoriesRepository(SeqDBCon);
			tagRep = new SeqTagsRepository(SeqDBCon);
      }

      #endregion

      #region IMetaWeblog Members

      /// <summary>
      /// Add a new post to the blog.
      /// </summary>
      /// <param name="blogid">The name of the blog.</param>
      /// <param name="username">aspnet_Membership.UserId </param>
      /// <param name="password">aspnet_Membership.Password</param>
      /// <param name="post">Contents of post from WLW2011</param>
      /// <param name="publish">False if publishing a draft.</param>
      /// <returns>The post id of the new post.</returns>
      string IMetaWeblog.AddPost(string blogid, string username, 
			string password, Post post, bool publish) {

			if (InvalidUser(username, password))
				throw new XmlRpcFaultException(INVALID_CREDENTIALS,
														INVALID_CREDENTIALS_MSG);

         return postsRep.SavePost(blogid, post, publish);
      }

      bool IMetaWeblog.UpdatePost(string postid, string username, string password,
          Post post, bool publish) {

			if (InvalidUser(username, password))
				throw new XmlRpcFaultException(INVALID_CREDENTIALS,
														INVALID_CREDENTIALS_MSG);
         //postid is null from parameters so seed it with postid
         post.postid = postid;
         return postsRep.UpdatePost(post, publish);
      }

      Post IMetaWeblog.GetPost(string postid, string username, string password) {
			if (InvalidUser(username, password))
				throw new XmlRpcFaultException(INVALID_CREDENTIALS,
														INVALID_CREDENTIALS_MSG);

         SeqPost sp = postsRep.GetPost(Convert.ToInt32(postid))
										.SingleOrDefault<SeqPost>();
         Post post = new Post();
         post.postid = sp.PostId;
         post.description = sp.Description;
         post.title = sp.Title;
         post.dateCreated = sp.CreateDate;
         post.mt_excerpt = sp.Excerpt;
         post.mt_text_more = sp.ExtendedText;
         post.mt_keywords = tagRep.GetKeywords(sp.PostId);
         post.categories = new string[] { sp.SeqCategory.Name };
         return post;
      }

      CategoryInfo[] IMetaWeblog.GetCategories(string blogid, 
                                               string username, 
                                               string password) {
         if (InvalidUser(username, password))
				throw new XmlRpcFaultException(INVALID_CREDENTIALS, 
														 INVALID_CREDENTIALS_MSG);

         List<CategoryInfo> categoryInfos = new List<CategoryInfo>();
         IQueryable<SeqCategory> cats = catRep.AllCategories(blogid);
         foreach (SeqCategory sc in cats) {
            CategoryInfo ci = new CategoryInfo();
            ci.categoryid = sc.CategoryId.ToString();
            ci.title = sc.Name;
            ci.description = "None";
            ci.htmlUrl = "Not implemented";
            ci.rssUrl = "Not implemented";
            categoryInfos.Add(ci);
         }
         return categoryInfos.ToArray();
      }

      Post[] IMetaWeblog.GetRecentPosts(string blogid, string username,
         string password, int numberOfPosts) {

			if (InvalidUser(username, password))
				throw new XmlRpcFaultException(INVALID_CREDENTIALS,
														INVALID_CREDENTIALS_MSG);
         List<Post> posts = new List<Post>();
         IQueryable<SeqPost> recentPosts =
            postsRep.GetPostPage(numberOfPosts,1,blogid);

         foreach (SeqPost sp in recentPosts) {
            Post p = new Post();
            p.postid = sp.PostId;
            p.title = sp.Title;
            p.dateCreated = sp.CreateDate;
            p.description = sp.Description;
            string[] category = new string[1];
            category[0] = (sp.SeqCategory == null) 
                           ? "" : sp.SeqCategory.Name;
            p.categories = category;
            posts.Add(p);
         }
         return posts.ToArray();
      }

      MediaObjectInfo IMetaWeblog.NewMediaObject(string blogid, 
			string username, string password, MediaObject mediaObject) {
			
			if (InvalidUser(username, password))
				throw new XmlRpcFaultException(INVALID_CREDENTIALS,
														INVALID_CREDENTIALS_MSG);         
         MediaObjectInfo objectInfo = new MediaObjectInfo();
         string mediaFileName = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory + "\\Content",
            mediaObject.name);
         string justDir = Path.GetDirectoryName(mediaFileName);
			try {
				EnsureDirectory(new DirectoryInfo(justDir));
				File.WriteAllBytes(mediaFileName, mediaObject.bits);
			}
			catch (SecurityException se) {
				throw new XmlRpcFaultException(MEDIAOBJECT_PERMISSION_FAILURE,
					"Permission to write " + mediaFileName + "denied. Underlying "+
					"error: " +se.Message);
			}
			catch (IOException ioe) {
				throw new XmlRpcFaultException(MEDIAOBJECT_IO_FAILURE,
					"Could not write " + mediaFileName + ". Underlying error: " +
					ioe.Message);
			}
			string appPath = Context.ApplicationInstance.Request.ApplicationPath;
			/* When installed in the root dir of server the appPath="/". This
			   makes the url be //Content/mediaobject.name which causes this:
			   http://Content/mediaObject.name on production. */
			if (appPath.Length > 1) appPath += "/";
			objectInfo.url = appPath+"Content/" + mediaObject.name;
         return objectInfo;
      }

      private static void EnsureDirectory(DirectoryInfo oDirInfo) {
         if (!oDirInfo.Exists) oDirInfo.Create();
      }

      /* Do not implement without reasonable security. */
      bool IMetaWeblog.DeletePost(string key, string postid, string username,
			string password, bool publish) {

			if (InvalidUser(username, password))
				throw new XmlRpcFaultException(INVALID_CREDENTIALS,
														 INVALID_CREDENTIALS_MSG);

			throw new XmlRpcFaultException(METHOD_NOT_ACCESSIBLE, 
				METHOD_NOT_ACCESSIBLE_MSG);
      }

      BlogInfo[] IMetaWeblog.GetUsersBlogs(string key, string username, string password) {
			if (InvalidUser(username, password))
				throw new XmlRpcFaultException(INVALID_CREDENTIALS,
														 INVALID_CREDENTIALS_MSG);
         List<BlogInfo> infoList = new List<BlogInfo>();
         BlogInfo b = new BlogInfo();
			b.blogName = ConfigurationManager.AppSettings["BlogName"];
         b.blogid = ConfigurationManager.AppSettings["BlogId"];
			b.url = ConfigurationManager.AppSettings["BlogUrl"]; ;
         infoList.Add(b);
         return infoList.ToArray();
      }

      UserInfo IMetaWeblog.GetUserInfo(string key, string username, string password) {
			if (InvalidUser(username, password))
				throw new XmlRpcFaultException(INVALID_CREDENTIALS,
														 INVALID_CREDENTIALS_MSG);
         UserInfo info = new UserInfo();
         return info;
      }

      Author[] IMetaWeblog.GetAuthors(string blogid, string username, string password) {
			if (InvalidUser(username, password))
				throw new XmlRpcFaultException(INVALID_CREDENTIALS,
														 INVALID_CREDENTIALS_MSG);
         Author theAuthor = new Author();
         theAuthor.display_name = "Steamee";
         theAuthor.user_email = "steamee@gmail.com";
         theAuthor.user_login = "4545";
         theAuthor.user_id = 42;
         theAuthor.meta_value = "hello";
         Author[] writers = new Author[1];
         writers[0] = theAuthor;
         return writers;
      }
      #endregion

      #region Private Methods

		private bool InvalidUser(string username, string password) {
			return !Membership.ValidateUser(username, password);
		}

      #endregion
   }
}
