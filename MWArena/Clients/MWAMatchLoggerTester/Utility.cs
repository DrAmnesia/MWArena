using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace MwoA.Util
{
    public static class HttpUtility
    {
        public static object MakeRequest(string requestUrl, object JSONRequest, string JSONmethod,
                                         string JSONContentType, Type JSONResponseType)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                //WebRequest WR = WebRequest.Create(requestUrl);

                string sb = JsonConvert.SerializeObject(JSONRequest);

                request.Method = (JSONmethod == "" | JSONmethod == null) ? "POST" : JSONmethod;
                request.ContentType = (JSONmethod == "" | JSONmethod == null) ? "application/json" :  JSONContentType; // "application/json";

                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();


                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Found)
                        throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).",
                            response.StatusCode,
                            response.StatusDescription));
                    //  DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                    // object objResponse = JsonConvert.DeserializeObject();
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    object objResponse = JsonConvert.DeserializeObject(strsb, JSONResponseType);

                    return objResponse;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static object MakeRequest<T>(this T JSONResponseType,string requestUrl, object JSONRequest, string JSONmethod,
                                    string JSONContentType)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                //WebRequest WR = WebRequest.Create(requestUrl);

                string sb = JsonConvert.SerializeObject(JSONRequest);

                request.Method = JSONmethod; // "POST";
                request.ContentType = JSONContentType; // "application/json";

                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();


                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).",
                            response.StatusCode,
                            response.StatusDescription));
                    //  DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                    // object objResponse = JsonConvert.DeserializeObject();
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    object objResponse = JSONResponseType.DeserializeJsonObject(strsb);

                    return objResponse;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public static T DeserializeJsonObject<T>(this T JsonObjectType, string JsonString) { return JsonConvert.DeserializeObject<T>(JsonString); }

        public static void WatchDir(string dirname)
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = dirname;
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite ;
            // Only watch text files.
            watcher.Filter = "*.csv";

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
    

            // Begin watching.
            watcher.EnableRaisingEvents = true;
            
        }

        // Define the event handlers. 
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            //get the file content that has changed 
            //populate a matchresults object for each line
            //push the new objects to the matchresultstable

            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
        }
    }
}