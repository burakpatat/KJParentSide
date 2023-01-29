using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Environments._Mutual.Connection;
using _Environments._Mutual.Data.State;
using SimpleJSON;
using System.Linq;

namespace _Environments._Mutual.Data
{
    public abstract class AbstractGetData
    {
        public static string _GETResponseResult = "";
        public static string _POSTResponseResult = "";

        protected static string Get_SubUrl(GetTarget target, string[] args, string subUrl)
        {

            string extension = "";
            switch (target)
            {
                case GetTarget.BASE:
                    extension += subUrl;
                    break;
                case GetTarget.FILE:
                    extension += "/assets/";
                    break;
                default:
                    break;

            }

            return extension;
        }
        protected static string Post_SubUrl(PostTarget target, string[] args, string subUrl)
        {

            string extension = "";
            switch (target)
            {
                case PostTarget.NEWBASE:
                    extension += subUrl;
                    break;

                default:
                    break;

            }

            return extension;
        }
        protected static string Update_SubUrl(UpdateTarget target, string subUrl)
        {

            string extension = "";
            switch (target)
            {
                case UpdateTarget.UPDATEBASE:
                    extension += subUrl;
                    break;

                default:
                    break;

            }

            return extension;
        }
        protected static string Delete_SubUrl(DeleteTarget target, string subUrl)
        {

            string extension = "";
            switch (target)
            {
                case DeleteTarget.DELETEBASE:
                    extension += subUrl;
                    break;

                default:
                    break;

            }

            return extension;
        }
        protected static IEnumerator GetResultResponse(GetTarget target, string[] args, string mainUrl)
        {
            string result = "";
            RequestHelper basichelper = new RequestHelper();

            basichelper.Uri = mainUrl;
            basichelper.Method = "GET";
            basichelper.Headers = new Dictionary<string, string>();
            basichelper.Headers.Add("Authorization", "Bearer " + ConnectionManager.Instance.ActiveToken);
            basichelper.Timeout = 25;


            RestClient.Request(basichelper).Then(response =>
            {
                if (response.Text == "")
                {
                    Error(new System.Exception("Empty response"), "Received empty response.");
                }
                else
                {
                    Debug.Log("Get response: " + response.Text);
                    result = response.Text;
                }
            }).Catch(err =>
            {
                var error = err as RequestException;
                Debug.Log(err.Message + " st:" + error.Response);
                result = error.Response;
            });


            yield return new WaitUntil(() => result != "");

            _GETResponseResult = result;
        }
        protected static IEnumerator PostData(string mainUrl, PostTarget target, object RequestData) //Restclient version
        {
            string result = "";
            RequestException basicexception = new RequestException();
            RequestHelper basichelper = new RequestHelper();

            basichelper.Uri = mainUrl;
            basichelper.Method = "POST";
            basichelper.Headers = new Dictionary<string, string>();
            basichelper.Timeout = 25;
            basichelper.Headers.Add("Authorization", "Bearer " + ConnectionManager.Instance.ActiveToken);

            basichelper.Body = RequestData;
            basichelper.BodyString = Newtonsoft.Json.JsonConvert.SerializeObject(RequestData);
            basichelper.ContentType = "application/json; charset=utf-8";

            RestClient.Request(basichelper).Then(response =>
            { 
                Debug.Log("Get response: " + response.Text);
                result = response.Text;
            }).Catch(err =>
            {
                var error = err as RequestException;
                Debug.Log(err.Message + " st:" + error.Response);
                result = error.Response;
            });

            yield return new WaitUntil(() => result != "");
            
            _POSTResponseResult = result;

        }
        protected static IEnumerator UpdateData(string mainUrl, UpdateTarget target, object RequestData)
        {
            string result = "";
            RequestHelper basichelper = new RequestHelper();

            basichelper.Uri = mainUrl;
            basichelper.Method = "PATCH";
            basichelper.Headers = new Dictionary<string, string>();
            basichelper.Timeout = 25;
            basichelper.Headers.Add("Authorization", "Bearer " + ConnectionManager.Instance.ActiveToken);

            basichelper.Body = RequestData;
  
            basichelper.BodyString = Newtonsoft.Json.JsonConvert.SerializeObject(RequestData);
            basichelper.ContentType = "application/json; charset=utf-8";

            RestClient.Patch(basichelper).Then(response =>
            {
                Debug.Log("Get response: " + response.Text);
                result = response.Text;
            }).Catch(err =>
            {
                var error = err as RequestException;
                Debug.Log(err.Message + " st:" + error.Response);
                result = error.Response;
            });

            yield return new WaitUntil(() => result != "");
        }
        protected static IEnumerator DeleteData(string mainUrl, DeleteTarget target, object RequestData)
        {
            string result = "";
            RequestHelper basichelper = new RequestHelper();

            basichelper.Uri = mainUrl;
            basichelper.Method = "DELETE";
            basichelper.Headers = new Dictionary<string, string>();
            basichelper.Timeout = 25;
            basichelper.Headers.Add("Authorization", "Bearer " + ConnectionManager.Instance.ActiveToken);

            basichelper.Body = RequestData;

            basichelper.BodyString = Newtonsoft.Json.JsonConvert.SerializeObject(RequestData);
            basichelper.ContentType = "application/json; charset=utf-8";

            RestClient.Delete(basichelper).Then(response =>
            {
                Debug.Log("Get response: " + response.Text);
                result = response.Text;
            }).Catch(err =>
            {
                var error = err as RequestException;
                Debug.Log(err.Message + " st:" + error.Response);
                result = error.Response;
            });

            yield return new WaitUntil(() => result != "");
        }
        public static void Error(System.Exception Error, string CategoryM)
        {
            Debug.Log("Error: " + Error.Message + Error.StackTrace + "\n" + Error.InnerException?.Message + Error.InnerException?.StackTrace);
        }
    }
    public enum GetTarget { BASE, FILE }
    public enum PostTarget { NEWBASE }
    public enum UpdateTarget { UPDATEBASE }
    public enum DeleteTarget { DELETEBASE }
}
