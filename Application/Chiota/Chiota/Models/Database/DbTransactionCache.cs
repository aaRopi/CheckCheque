﻿using System;
using System.Collections.Generic;
using System.Text;
using Chiota.Models.Database.Base;
using Newtonsoft.Json;

namespace Chiota.Models.Database
{
    public class DbTransactionCache : TableModel
    {
        [JsonProperty("transactionhash")]
        public string TransactionHash { get; set; }

        [JsonProperty("chataddress")]
        public string ChatAddress { get; set; }

        [JsonProperty("messagetryte")]
        public string MessageTryte { get; set; }
    }
}
