using Android.Content;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace IT123P_FinalMP
{
    internal class NextActivityHandler
    {
        private Context context;
        private string toastMsg;
        private Type currClass;
        private Dictionary<string, string> dataDictionary;
        private string studNum;

        public NextActivityHandler(Context context, string toastMsg, Type currClass)
        {
            this.context = context;
            this.toastMsg = toastMsg;
            this.currClass = currClass;
            this.dataDictionary = new Dictionary<string, string>();
        }

        public NextActivityHandler(Context context, string studNum, Type currClass, string toastMsg)
        {
            this.context = context;
            this.toastMsg = toastMsg;
            this.currClass = currClass;
            this.studNum = studNum;
            this.dataDictionary = new Dictionary<string, string>();
        }

        // Navigates to the next activity
        public void NavigateToNextActivity(Context context)
        {
            Intent intent = new Intent(context, currClass);

            // Add all data items from dataDictionary to the Intent
            foreach (var kvp in dataDictionary)
            {
                intent.PutExtra(kvp.Key, kvp.Value);

            }

            context.StartActivity(intent);

        }



        // Adds a key-value pair to the data dictionary
        public void PassDataToNextActivity(string key, string value)
        {
            dataDictionary[key] = value;
        }

        public string RetrieveDataFromIntent(string key)
        {
            return dataDictionary[key];
        }


        // Retrieves data from the intent and returns as a dictionary
        public Dictionary<string, string> RetrieveDataFromIntent(Intent intent)
        {
            Dictionary<string, string> retrievedData = new Dictionary<string, string>();

            foreach (string key in dataDictionary.Keys)
            {
                string value = intent.GetStringExtra(key);
                if (value != null)
                {
                    retrievedData[key] = value;
                }
            }

            return retrievedData;
        }
    }
}