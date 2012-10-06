using CookComputing.XmlRpc;
using Sequential2013.Domain.Models;

namespace Sequential2013.Domain.Abstract
{
   /// <summary>
   /// Taken from Scott Hanselman's blog who copied it from Keyvan Nayyeri
   /// http://www.hanselman.com/blog/TheWeeklySourceCode55NotABlogALocalXMLRPCMetaWebLogEndpointThatLiesToWindowsLiveWriter.aspx
   /// http://nayyeri.net/implement-metaweblog-api-in-asp-net
   /// </summary>
   public interface IMetaWeblog
   {
      #region MetaWeblog API

      [XmlRpcMethod("metaWeblog.newPost")]
      string AddPost(string blogid, string username, string password, Post post, bool publish);

      [XmlRpcMethod("metaWeblog.editPost")]
      bool UpdatePost(string postid, string username, string password, Post post, bool publish);

      [XmlRpcMethod("metaWeblog.getPost")]
      Post GetPost(string postid, string username, string password);

      [XmlRpcMethod("metaWeblog.getCategories")]
      CategoryInfo[] GetCategories(string blogid, string username, string password);

      [XmlRpcMethod("metaWeblog.getRecentPosts")]
      Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts);

      [XmlRpcMethod("metaWeblog.newMediaObject")]
      MediaObjectInfo NewMediaObject(string blogid, string username, string password,
          MediaObject mediaObject);

      #endregion

      #region Blogger API

      [XmlRpcMethod("blogger.deletePost")]
      [return: XmlRpcReturnValue(Description = "Returns true.")]
      bool DeletePost(string key, string postid, string username, string password, bool publish);

      [XmlRpcMethod("blogger.getUsersBlogs")]
      BlogInfo[] GetUsersBlogs(string key, string username, string password);

      [XmlRpcMethod("blogger.getUserInfo")]
      UserInfo GetUserInfo(string key, string username, string password);

      #endregion

      #region Wordpress API
      /// <summary>
      /// 
      /// </summary>
      /// <param name="blogid">
      /// Wordpress API defines the blog_id as an int but WLW2011 is clearly
      /// sending the blogid as a string. Would this be a problem for other
      /// blogging clients use with Sequential?
      /// </param>
      /// <param name="username"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      [XmlRpcMethod("wp.getAuthors")]
      Author[] GetAuthors(string blogid, string username, string password);
      #endregion
   }
}
