﻿using IdentityServer4.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.ConfigStore
{
    public class ClientEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }

        public string ClientData { get; set; }

        [Key]
        public string ClientId { get; set; }

        [NotMapped]
        public Client Client { get; set; }

        public void AddDataToEntity()
        {
            ClientData = JsonConvert.SerializeObject(Client);
            ClientId = Client.ClientId;
        }

        public void MapDataFromEntity()
        {
            Client = JsonConvert.DeserializeObject<Client>(ClientData);
            ClientId = Client.ClientId;
        }
    }
}
