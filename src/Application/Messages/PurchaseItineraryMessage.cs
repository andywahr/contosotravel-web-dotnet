using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ContosoTravel.Web.Application.Messages
{
    public class PurchaseItineraryMessage
    {
        [JsonProperty(PropertyName = "cartId")]
        public string CartId { get; set; }

        [JsonProperty(PropertyName = "purchasedOn")]
        public DateTimeOffset PurchasedOn { get; set; }
    }
}
