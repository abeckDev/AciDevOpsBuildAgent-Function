using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AciDevOpsBuildAgent_Function
{
    public static class RequestHelper
    {
        public static RequestBody GetRequestBody(HttpRequest req)
        {
            //Read POST Body and bind content to Model for further processing
            try
            {
                //Obtain parameters from the request
                return JsonConvert.DeserializeObject<RequestBody>(new StreamReader(req.Body).ReadToEnd());
            }
            catch (Exception ex)
            {
                //Complain if it wont work
                throw new Exception(ex.Message);
            }
        }
    }
}
