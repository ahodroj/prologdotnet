using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Prolog.CT
{ 
	/// <summary>
	/// Arguments class
	/// </summary>
	public class CommandLineArguments
	{
		// Variables
		private StringDictionary Parameters;


		// Constructor
		public CommandLineArguments(string[] Args)
		{
			Parameters = new StringDictionary();
			Regex Spliter = new Regex(@"^/|:",
				RegexOptions.IgnoreCase|RegexOptions.Compiled);

			Regex Remover = new Regex(@"^['""]?(.*?)['""]?$",
				RegexOptions.IgnoreCase|RegexOptions.Compiled);

			string Parameter = null;
			string[] Parts;

			// Examples: 
			// /param3:"Test" 
			foreach(string Txt in Args)
			{
				// Look for new parameters (-,/ or --) and a
				// possible enclosed value (=,:)
				Parts = Spliter.Split(Txt,3);

				switch(Parts.Length)
				{
						// Found a value (for the last parameter 
						// found (space separator))
					case 1:
						if(Parameter != null)
						{
							if(!Parameters.ContainsKey(Parameter)) 
							{
								Parts[0] = 
									Remover.Replace(Parts[0], "$1");

								Parameters.Add(Parameter, Parts[0]);
							}
							Parameter=null;
						}
						// else Error: no parameter waiting for a value (skipped)
						break;

						// Found just a parameter
					case 2:
						// The last parameter is still waiting. 
						// With no value, set it to true.
						if(Parameter!=null)
						{
							if(!Parameters.ContainsKey(Parameter)) 
								Parameters.Add(Parameter, "true");
						}
						Parameter=Parts[1];
						break;

						// Parameter with enclosed value
					case 3:
						// The last parameter is still waiting. 
						// With no value, set it to true.
						if(Parameter != null)
						{
							if(!Parameters.ContainsKey(Parameter)) 
								Parameters.Add(Parameter, "true");
						}

						Parameter = Parts[1];

						// Remove possible enclosing characters (",')
						if(!Parameters.ContainsKey(Parameter))
						{
							Parts[2] = Remover.Replace(Parts[2], "$1");
							Parameters.Add(Parameter, Parts[2]);
						}

						Parameter=null;
						break;
				}
			}
			// In case a parameter is still waiting
			if(Parameter != null)
			{
				if(!Parameters.ContainsKey(Parameter)) 
					Parameters.Add(Parameter, "true");
			}
		}

		public bool NoOptions() 
		{
			if(this.Parameters.Count == 1) 
			{
				if(this.Parameters["source"] != null) 
				{
					return true;
				}
			} 
			return false;
													  
		}
		public bool NoArgs() 
		{
			if(this.Parameters.Count == 0) 
			{
				return true;
			}
			return false;
		}
		public void Add(string key, string val) 
		{
			this.Parameters[key] = val;
		}
		// Retrieve a parameter value if it exists 
		// (overriding C# indexer property)
		public string this [string Param]
		{
			get
			{
				return(Parameters[Param]);
			}
		}
	}
}