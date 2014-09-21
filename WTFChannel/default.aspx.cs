//-----------------------------------------------------------------------
// <copyright file="default.aspx.cs" company="Andy Young">
//     Copyright (c) Andy Young. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace WTFChannel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.UI.WebControls;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using WTFChannel.DataContracts;

    /// <summary>
    /// Default page class
    /// </summary>
    public partial class DefaultPage : System.Web.UI.Page
    {
        /// <summary>
        /// JSON serializer
        /// </summary>
        private static readonly JsonSerializer Serializer = new JsonSerializer();

        /// <summary>
        /// Web request helper
        /// </summary>
        private static readonly WebRequestHelper WebRequestHelper = new WebRequestHelper();

        /// <summary>
        /// Page_Load event
        /// </summary>
        /// <param name="sender">What raised the event</param>
        /// <param name="e">Event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                HttpCookie cookie = null;
                TextBox[] values = new[] { this.PostalCode, this.CountryCode, this.ApiKey };
                foreach (var cookieValues in values)
                {
                    cookie = Request.Cookies[cookieValues.ID];
                    if (cookie != null)
                    {
                        cookieValues.Text = cookie.Value;
                    }
                }

                cookie = Request.Cookies["SelectedServiceId"];
                string selectedServiceId = cookie == null ? string.Empty : cookie.Value;
                cookie = Request.Cookies[ServiceProviders.ID];
                if (cookie != null)
                {
                    var cookieValues = cookie.Value.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var cookieValue in cookieValues)
                    {
                        var split = cookieValue.Split(new string[] { ":::" }, StringSplitOptions.RemoveEmptyEntries);
                        string value = split[0];
                        string name = split[1];
                        bool selected = value.Equals(selectedServiceId);

                        ServiceProviders.Items.Add(new ListItem() { Text = name, Value = value, Selected = selected });
                    }

                    ServiceProvidersPanel.Visible = true;
                }
            }
        }

        /// <summary>
        /// GetServiceProviders Click event
        /// </summary>
        /// <param name="sender">What raised the event</param>
        /// <param name="e">Event arguments</param>
        protected void GetServiceProviders_Click(object sender, EventArgs e)
        {
            string url = "http://api.rovicorp.com/TVlistings/v9/listings/services/postalcode/{0}/info?locale=en-US&countrycode={1}&apikey={2}&sig=sig&format=json";
            url = string.Format(url, PostalCode.Text, CountryCode.Text, this.ApiKey.Text);

            ServicesResultObject services = this.GetServiceProviders(url);
            if (services == null)
            {
                return;
            }

            GetServiceProvidersError.Visible = false;
            this.PopulateServiceProviders(services);
        }

        /// <summary>
        /// FindChannel Click event
        /// </summary>
        /// <param name="sender">What raised the event</param>
        /// <param name="e">Event arguments</param>
        protected void FindChannel_Click(object sender, EventArgs e)
        {
            string url = "http://api.rovicorp.com/TVlistings/v9/listings/gridschedule/{0}/info?apikey={1}&sig=sig&locale=en-US&duration=240&format=json";
            url = string.Format(url, ServiceProviders.SelectedValue, this.ApiKey.Text);

            GridScheduleObject shows = this.GetGridSchedule(url);
            this.PopulateGridSchedule(shows);
        }

        /// <summary>
        /// Gets the service providers
        /// </summary>
        /// <param name="url">Service provider URL</param>
        /// <returns>List of service providers</returns>
        private ServicesResultObject GetServiceProviders(string url)
        {
            ServicesResultObject services = null;
            object cached = Cache[url];
            if (cached == null)
            {
                HttpWebResponse webResponse;
                string response = this.GetResponse(url, out webResponse);
                if (this.HandleResponse(webResponse, response, this.GetServiceProvidersError))
                {
                    services = this.Deserialize<ServicesResultObject>(response);
                    Cache.Insert(url, services, null, DateTime.Now.AddDays(7), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }
            else
            {
                services = cached as ServicesResultObject;
            }

            return services;
        }

        /// <summary>
        /// Populates the service provides
        /// </summary>
        /// <param name="services">List of service providers</param>
        private void PopulateServiceProviders(ServicesResultObject services)
        {
            // Get the selected service if it exists
            HttpCookie selectedServiceCookie = Request.Cookies["SelectedServiceId"];
            string selectedServiceId = selectedServiceCookie == null ? string.Empty : selectedServiceCookie.Value;

            // Populate the services dropdown
            ServiceProviders.Items.Clear();
            List<string> cookieValues = new List<string>();
            foreach (var service in services.ServicesResult.Services.Service)
            {
                bool selected = service.ServiceId.Equals(selectedServiceId);
                ServiceProviders.Items.Add(new ListItem() { Text = service.Name, Value = service.ServiceId, Selected = selected });
                cookieValues.Add(string.Format("{0}:::{1}", service.ServiceId, service.Name));
            }

            if (selectedServiceCookie == null && services.ServicesResult.Services.Service.Count > 0)
            {
                // No selected service cookie existed -- save the first found service
                Response.Cookies.Add(new HttpCookie("SelectedServiceId", services.ServicesResult.Services.Service[0].ServiceId) { Expires = DateTime.MaxValue });
            }

            // Remember the found services
            Response.Cookies.Add(new HttpCookie(ServiceProviders.ID, string.Join("|||", cookieValues)) { Expires = DateTime.MaxValue });

            // Remember the user input
            TextBox[] values = new[] { PostalCode, CountryCode, this.ApiKey };
            foreach (var v in values)
            {
                Response.Cookies.Add(new HttpCookie(v.ID, v.Text) { Expires = DateTime.MaxValue });
            }

            ServiceProvidersPanel.Visible = true;
            ResultsPanel.Visible = false;
        }

        /// <summary>
        /// Gets the grid schedule
        /// </summary>
        /// <param name="url">Grid schedule URL</param>
        /// <returns>List of shows</returns>
        private GridScheduleObject GetGridSchedule(string url)
        {
            GridScheduleObject shows = null;
            object cached = Cache[url];
            if (cached == null)
            {
                HttpWebResponse webResponse;
                string response = this.GetResponse(url, out webResponse);
                if (this.HandleResponse(webResponse, response, this.FindChannelError))
                {
                    shows = this.Deserialize<GridScheduleObject>(response);
                    Cache.Insert(url, shows, null, DateTime.Now.AddDays(7), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }
            else
            {
                shows = cached as GridScheduleObject;
            }

            return shows;
        }

        /// <summary>
        /// Populates the grid schedule
        /// </summary>
        /// <param name="shows">List of shows</param>
        private void PopulateGridSchedule(GridScheduleObject shows)
        {
            FindChannelError.Visible = false;

            // Find any shows matching the show name query
            var query = from c in shows.GridScheduleResult.GridChannels
                        from a in c.Airings
                        where a.Title.IndexOf(ShowName.Text, StringComparison.OrdinalIgnoreCase) != -1
                        select new { Channel = c, Airing = a };

            if (query.Any())
            {
                ResultsPanel.Visible = true;

                // Save the client cookie to the request 
                HttpCookie cookie = Request.Cookies["SelectedServiceId"];
                if (cookie != null)
                {
                    Response.Cookies.Add(new HttpCookie("SelectedServiceId", cookie.Value) { Expires = DateTime.MaxValue });
                }
            }
            else
            {
                ResultsPanel.Visible = false;
            }

            int count = 0;
            foreach (var x in query)
            {
                // Parse out the air time
                DateTime parsedDate = DateTime.MinValue;
                if (DateTime.TryParse(x.Airing.AiringTime, out parsedDate))
                {
                    DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
                    parsedDate = parsedDate.ToLocalTime();
                }

                // Create the row
                TableRow row = new TableRow();
                row.Cells.AddRange(new TableCell[]
                {
                    new TableCell() { Text = x.Channel.Channel },
                    new TableCell() { Text = x.Channel.DisplayName },
                    new TableCell() { Text = parsedDate.ToString() },
                    new TableCell() { Text = x.Airing.Title },
                });

                if (count++ % 2 == 0)
                {
                    row.CssClass = "alt";
                }

                ResultsTable.Rows.Add(row);
            }
        }

        /// <summary>
        /// Deserialize the JSON response
        /// </summary>
        /// <typeparam name="T">Output type</typeparam>
        /// <param name="json">JSON response</param>
        /// <returns>Object based on type</returns>
        private T Deserialize<T>(string json) where T : class
        {
            T obj = null;

            Serializer.Converters.Add(new JavaScriptDateTimeConverter());
            Serializer.NullValueHandling = NullValueHandling.Ignore;

            obj = JsonConvert.DeserializeObject<T>(json);

            return obj;
        }

        /// <summary>
        /// Gets the response from the REST interface
        /// </summary>
        /// <param name="uri">Uri to which to send the request</param>
        /// <returns>Response string</returns>
        private string GetResponse(string uri)
        {
            HttpWebResponse response;
            string output = WebRequestHelper.GetResponse(uri, out response);
            return output;
        }

        /// <summary>
        /// Gets the response from the REST interface
        /// </summary>
        /// <param name="uri">Uri to which to send the request</param>
        /// <param name="response">Web response</param>
        /// <returns>Response string</returns>
        private string GetResponse(string uri, out HttpWebResponse response)
        {
            string output = WebRequestHelper.GetResponse(uri, out response);
            return output;
        }

        /// <summary>
        /// Handles the web response
        /// </summary>
        /// <param name="response">Web response</param>
        /// <param name="responseString">Web response string</param>
        /// <param name="errorLabel">Error label to set</param>
        /// <returns>True if OK, false otherwise</returns>
        private bool HandleResponse(HttpWebResponse response, string responseString, Label errorLabel)
        {
            errorLabel.Visible = false;
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return true;
                case HttpStatusCode.NotModified:
                    return true;
                default:
                    this.ShowError(errorLabel, responseString);
                    return false;
            }
        }

        /// <summary>
        /// Shows the given error
        /// </summary>
        /// <param name="errorLabel">Error label to set</param>
        /// <param name="message">String to use</param>
        private void ShowError(Label errorLabel, string message)
        {
            errorLabel.Text = message;
            errorLabel.Visible = true;
        }
    }
}