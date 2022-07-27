using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace CommercialGateAPI.Response
{
    /// <summary>
    /// Standard response model which will return pay load with response  status Code
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseModel<T> : IHttpActionResult
    {
        private HttpRequestMessage HttpRequestMessage { get; set; }
        private HttpResponseMessage HttpResponseMessage { get; set; }
        private HttpStatusCode StatusCode { get; set; }
        private string Message { get; set; }
        private List<T> ResponseListPayload { get; set; }
        private T ResponsePayload { get; set; }

        /// <summary>
        ///  Response is with status Code and response payload
        /// </summary>
        /// <param name="request"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        public ResponseModel(HttpRequestMessage request, HttpStatusCode statusCode, string message) //response for exception 
        {
            this.HttpRequestMessage = request;
            this.StatusCode = statusCode;
            this.Message = message;
        }

        /// <summary>
        /// Response is with status Code and response payload
        /// </summary>
        /// <param name="RequestPayload"></param>
        /// <param name="request"></param>
        public ResponseModel(T RequestPayload, HttpRequestMessage request) //response for PayLoad 
        {
            ResponsePayload = RequestPayload;
            HttpRequestMessage = request;
        }
        /// <summary>
        ///  Response is with status Code and response payload
        /// </summary>
        /// <param name="RequestPayload"></param>
        /// <param name="request"></param>
        public ResponseModel(List<T> RequestPayload, HttpRequestMessage request)//response for List<> PayLoad 
        {
            ResponseListPayload = RequestPayload;
            HttpRequestMessage = request;
        }

        /// <summary>
        ///  Response is with status Code and response payload
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            if (ResponsePayload == null && ResponseListPayload != null)
            {
                HttpResponseMessage = HttpRequestMessage.CreateResponse(HttpStatusCode.OK, ResponseListPayload);
            }
            else if (ResponsePayload != null && ResponseListPayload == null)
            {
                HttpResponseMessage = HttpRequestMessage.CreateResponse(HttpStatusCode.OK, ResponsePayload);
            }
            else if (this.StatusCode == HttpStatusCode.InternalServerError)
            {
                HttpResponseMessage = HttpRequestMessage.CreateResponse(HttpStatusCode.InternalServerError, this.Message);
            }
            //else if (ResponsePayload == null && ResponseListPayload == null)
            //{
            //    //HttpResponseMessage = HttpRequestMessage.CreateResponse(HttpStatusCode.InternalServerError);
            //    HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            //    {
            //        RequestMessage = HttpRequestMessage,
            //        ReasonPhrase = "InternalServerError"
            //    };
            //}
            else
            {
                HttpResponseMessage = HttpRequestMessage.CreateResponse(HttpStatusCode.NoContent);
            }
            return Task.FromResult(HttpResponseMessage);
        }
    }
}