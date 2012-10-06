using System.Text.RegularExpressions;

namespace Sequential2013.Domain.Utilities
{
	/// <summary>
	/// Utility class for working with permalinks.
	/// </summary>
	public sealed class Permalink
	{
		/// <summary>
		/// Generates a permalink by removing all characters that are not a-zA-Z0-9
		/// </summary>
		/// <param name="s">The string to convert to a permalink. Usually this is the
		/// title of a post.</param>
		/// <returns>Permalink string which could be empty if all characters have
		/// been replaced.</returns>
		public  static string Generate(string s)
		{
			Regex spc = new Regex("\\s");
			Regex reg = new Regex("[^a-zA-Z0-9-"+Regex.Escape(" !()_.")+"]");
			string link = spc.Replace(s,"-"); //Spaces to dash
			link = reg.Replace(link,""); //Strip characters which may make problem URLs
			return link;
		}
	} //end class
} //end namespace
