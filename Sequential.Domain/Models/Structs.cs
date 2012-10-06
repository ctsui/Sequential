using System;
using CookComputing.XmlRpc;

//Copiedfrom http://nayyeri.net/implement-metaweblog-api-in-asp-net
namespace Sequential2013.Domain.Models {
   #region Structs

   public struct BlogInfo {
      public string blogid;
      public string url;
      public string blogName;
   }

   public struct Category {
      public string categoryId;
      public string categoryName;
   }

   [Serializable]
   public struct CategoryInfo {
      public string description;
      public string htmlUrl;
      public string rssUrl;
      public string title;
      public string categoryid;
   }

   [XmlRpcMissingMapping(MappingAction.Ignore)]
   public struct Enclosure {
      public int length;
      public string type;
      public string url;
   }

   [XmlRpcMissingMapping(MappingAction.Ignore)]
   public struct Post {
      public DateTime dateCreated;
      public string description;
      public string title;
      public string[] categories;
      public string permalink;
      public object postid;
      public string userid;
      /* The wp API has this an int but WLW2011 sends stringified id */
      public string wp_author_id;
      public int mt_allow_comments;
      public string mt_text_more;
      public string mt_excerpt;
      public string mt_keywords;
   }


   [XmlRpcMissingMapping(MappingAction.Ignore)]
   public struct Source {
      public string name;
      public string url;
   }

   public struct UserInfo {
      public string userid;
      public string firstname;
      public string lastname;
      public string nickname;
      public string email;
      public string url;
   }

   [XmlRpcMissingMapping(MappingAction.Ignore)]
   public struct MediaObject {
      public string name;
      public string type;
      public byte[] bits;
   }

   [Serializable]
   public struct MediaObjectInfo {
      public string url;
   }

   [Serializable]
   public struct Author {
      public int user_id;
      public string user_login;
      public string display_name;
      public string user_email;
      public string meta_value;
   }

   #endregion
}
