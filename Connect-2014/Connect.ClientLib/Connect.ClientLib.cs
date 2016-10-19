// Copyright © 2014 Mimeo, Inc.

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace Connect.ClientLib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Connect.ClientLib.Helpers;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Net.Security;
    using System.Web.Script.Serialization;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using System.Xml;
    using System.Xml.Linq;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Provides access to the Mimeo Connect REST API.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class ConnectClientLib : IConnectClientLib
    {
        #region State

        /// <summary>
        /// The base URI of the Mimeo Connect REST service.
        /// </summary>
        /// <remarks></remarks>
        protected static string BaseUri = EnumHelper.GetDescription(ServiceURI.SandboxUS);

        /// <summary>
        /// Gets or sets the HTTP implementation.
        /// </summary>
        protected HttpClient WorkerDelme { get; set; }

        /// <summary>
        /// Gets or sets the factory used to construct response objects.
        /// </summary>
        protected IResponseFactory ResponseFactory { get; set; }

        protected string authInformation { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SendWithUsClient class.
        /// </summary>
        /// <param name="apiKey">The API key to use when authenticating with SendWithUs.</param>
        public ConnectClientLib(string user, string password)
            : this(user, password, new HttpClient())
        { }

        /// <summary>
        /// Initializes a new instance of the SendWithUsClient class.
        /// </summary>
        /// <param name="apiKey">The API key to use when authenticating with SendWithUs.</param>
        /// <param name="worker">The HTTP implementation.</param>
        public ConnectClientLib(string user, string password, HttpClient worker)
            : this(user, password, worker, new ResponseFactory())
        { }

        /// <summary>
        /// Initializes a new instance of the SendWithUsClient class.
        /// </summary>
        /// <param name="apiKey">The API key to use when authenticating with SendWithUs.</param>
        /// <param name="worker">The HTTP implementation.</param>
        /// <param name="responseFactory">The factory used to construct response objects.</param>
        public ConnectClientLib(string user, string password, HttpClient worker, IResponseFactory responseFactory)
        {
            this.Initialize(user, password, worker, responseFactory);
        }

        /// <summary>
        /// Prepares the current instance for use.
        /// </summary>
        /// <param name="apiKey">The API key to use when authenticating with SendWithUs.</param>
        /// <param name="worker">The HTTP implementation.</param>
        /// <param name="responseFactory">The factory used to construct response objects.</param>
        /// <returns>The current instance.</returns>
        protected virtual ConnectClientLib Initialize(string user, string password, HttpClient worker, IResponseFactory responseFactory)
        {
            EnsureArgument.NotNullOrEmpty(user, "user");
            EnsureArgument.NotNullOrEmpty(password, "password");
            EnsureArgument.NotNull(worker, "worker");
            EnsureArgument.NotNull(responseFactory, "responseFactory");

            ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            this.authInformation = this.BuildAuthenticationToken(user, password);

            this.WorkerDelme = worker;


            return this;
        }

        #endregion

        #region Methods
        public void SetBaseEndPoint(ServiceURI serviceURI)
        {
            BaseUri = EnumHelper.GetDescription(serviceURI);
        }
        public string getBaseEndPoint()
        {
            return BaseUri;
        }
        public T ConvertToObject<T>(JObject json)
        {
            EnsureArgument.NotNull(json, "json");

            T request = json.ToObject<T>();

            return request;
        }
        private StreamContent CreateFileContent(Stream stream, string fileName, string contentType)
        {
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"files\"",
                FileName = "\"" + fileName + "\""
            }; // the extra quotes are key here
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return fileContent;
        }
        #endregion

        #region API methods

        public async Task<JObject> GetJsonRequest(String requestName)
        {
            EnsureArgument.NotNull(requestName, "requestName");

            var httpResponse = await this.GetJsonAsync(requestName);
            var json = await this.ReadJsonAsync(httpResponse);

            return json;
        }

        public async Task<XDocument> GetRequest(String requestName)
        {
            EnsureArgument.NotNull(requestName, "requestName");

            var httpResponse = await this.GetXmlAsync(requestName);
            var xml = await this.ReadXmlAsync(httpResponse);

            return xml;
        }

        public async Task<JObject> PostJsonRequest<T>(String requestName, T request)
        {
            EnsureArgument.NotNull(requestName, "requestName");
            EnsureArgument.NotNull(request, "request");

            //Should validate before we send ...
            var httpResponse = await this.PostJsonAsync(requestName, request);
            var json = await this.ReadJsonAsync(httpResponse);

            return json;
        }

        public async Task<XDocument> PostRequest<T>(String requestName, T request)
        {
            EnsureArgument.NotNull(requestName, "requestName");
            EnsureArgument.NotNull(request, "request");

            //Should validate before we send ...           
            var httpResponse = await this.PostJsonAsync(requestName, request);
            var xml = await this.ReadXmlAsync(httpResponse);

            return xml;
        }

        public async Task<JArray> PostasXmlRequest<T>(String requestName, T request)
        {
            EnsureArgument.NotNull(requestName, "requestName");
            EnsureArgument.NotNull(request, "request");

            //Should validate before we send ...           
            var httpResponse = await this.PostJsonAsync(requestName, request);
            var json = await this.ReadJsonArrayAsync(httpResponse);

            return json;
        }

        async public Task<HttpResponseMessage> UploadPDF(string foldername, string filename, byte[] byteData)
        {
            using (var client = newClient(true))
            {
                using (var content = new MultipartFormDataContent())
                {
                    Stream stream = new MemoryStream(byteData);
                    content.Add(CreateFileContent(stream, filename, "application/octet-stream"));
                    string endpointUri = string.Format("StorageService/{0}", foldername);
                    return await client.PostAsync(endpointUri, content);
                }
            }
        }

        async public Task<Guid> UploadXmlPDF(string foldername, string filename, byte[] byteData)
        {
            using (var client = newClient(true))
            {
                using (var content = new MultipartFormDataContent())
                {
                    Stream stream = new MemoryStream(byteData);
                    content.Add(CreateFileContent(stream, filename, "application/octet-stream"));
                    string endpointUri = string.Format("StorageService/{0}", foldername);
                    var httpResponse = await client.PostAsync(endpointUri, content);
                    var fileXml = await this.ReadXmlAsync(httpResponse);
                    String fileId = (from file in fileXml.Descendants(NameSpaces.ns + "File")
                                     select file.Element(NameSpaces.ns + "FileId").Value).FirstOrDefault();

                    return Guid.Parse(fileId);
                }
            }
        }



        #endregion

        #region Helpers

        /// <summary>
        /// Builds a token for use in HTTP basic authentication.
        /// </summary>
        /// <param name="apiKey">A valid SendWithUs API key.</param>
        /// <returns>A token for use in HTTP basic authentication.</returns>
        protected string BuildAuthenticationToken(string user, string password)
        {
            var userName_password = user + ":" + password;
            byte[] encDataByte = System.Text.Encoding.UTF8.GetBytes(userName_password);
            return Convert.ToBase64String(encDataByte);
        }

        /// <summary>
        /// Builds a URI string from the given arguments.
        /// </summary>
        /// <param name="template">A URI template with positional parameters.</param>
        /// <param name="args">Arguments for the template.</param>
        /// <returns>The expanded template.</returns>
        /// <remarks>The template MUST NOT begin with a slash.</remarks>
        protected string BuildRequestUri(string path, params object[] args)
        {
            return ConnectClientLib.BaseUri + (args.Length > 0 ? String.Format(path, args) : path);
        }

        protected async Task<HttpResponseMessage> GetJsonAsync(string path)
        {
            using (var client = newClient(false))
            {
                return await client.GetAsync(path);
            }
        }

        protected async Task<HttpResponseMessage> GetXmlAsync(string path)
        {
            using (var client = newClient(true))
            {
                return await client.GetAsync(path);
            }
        }

        protected async Task<HttpResponseMessage> PostXmlAsync<T>(string path, T request)
        {
            using (var client = newClient(true))
            {
                return await client.PostAsXmlAsync(this.BuildRequestUri(path), request);
            }
        }


        protected async Task<HttpResponseMessage> PostJsonAsync<T>(string path, T request)
        {
            using (var client = newClient(false))
            {
                return await client.PostAsJsonAsync<T>(this.BuildRequestUri(path), request).ConfigureAwait(false);
            }
        }

        protected Task<JObject> ReadJsonAsync(HttpResponseMessage response)
        {
            return response.Content.ReadAsAsync<JObject>();
        }

        protected Task<JArray> ReadJsonArrayAsync(HttpResponseMessage response)
        {
            return response.Content.ReadAsAsync<JArray>();
        }

        protected Task<XDocument> ReadXmlAsync(HttpResponseMessage response)
        {
            XDocument doc;
            return response.Content.ReadAsStreamAsync().ContinueWith(
                (streamTask) =>
                {
                    doc = XDocument.Load(streamTask.Result);
                    return doc;
                });
        }

        protected bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }

        private HttpClient newClient(bool xml)
        {

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", authInformation);

            if (xml)
            {

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/xml")
                    );
            }
            else
            {
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                    );
            }



            client.BaseAddress = new Uri(BaseUri);

            client.Timeout = new TimeSpan(0, 5, 0);

            return client;

        }
        #endregion
    }
}
