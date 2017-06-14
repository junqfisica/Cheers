using UnityEngine;
using System.Collections;

namespace Repository.Data{


	public static class DataStructure{
		/// <summary>
		/// Gets the user data structure {ID, Name, Age, Email, Gender};.
		/// </summary>
		/// <value>The user data structure.</value>
		public static string[] userDataStructure{get {
				string[] structure = {"ID","Name", "Age", "Gender"};
				return structure;
			}
		}

		/// <summary>
		/// Gets the prefs data structure {ID, Smoke, MinAge, MaxAge, Habit, WantToMeetMan, WantToMeetWoman};.
		/// </summary>
		/// <value>The prefs data structure.</value>
		public static string[] prefsDataStructure{get{

				string[] structure = {"ID","Smoke", "MinAge", "MaxAge", "Habit", "WantToMeetMan", "WantToMeetWoman"};
				return structure;
			}
		}
		/// <summary>
		/// Gets the matchs data structure {Number, Chooser_ID, Chosen_ID, Cheers}.
		/// </summary>
		/// <value>The matchs data structure.</value>
		public static string[] matchsDataStructure{get{

				string[] structure = {"Number", "Chooser_ID", "Chosen_ID", "Cheers"};
				return structure;
			}
		}

	}

}
