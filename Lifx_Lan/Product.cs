using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    /// <summary>
    /// Info pulled from https://github.com/LIFX/products/blob/master/products.json
    /// </summary>
    internal class Product
    {
        public int Vendor_ID { get; } = 0;

        public string Vendor_Name { get; } = "";

        public int Product_ID { get; } = 0;

        public string Product_Name { get; } = "";

        public int Firmware_Major { get; } = 0;

        public int Firmware_Minor { get; } = 0;

        public bool HEV { get; } = false;

        public bool Color { get; } = false;

        public bool Chain { get; } = false;

        public bool Matrix { get; } = false;

        public bool Relays { get; } = false;

        public bool Buttons { get; } = false;

        public bool Infrared { get; } = false;

        public bool Multizone { get; } = false;

        public int[] Temperature_Range { get; } = new int[2];

        public bool Extended_Multizone { get; } = false;

        public Product(/*int vendor_id, int product_id, int firmware_major, int firmware_minor*/)
        {
            /*Vendor_ID = vendor_id;              //GetVersion
            Product_ID = product_id;            //GetVersion
            Firmware_Major = firmware_major;    //GetHostFirmware
            Firmware_Minor = firmware_minor;    //GetHostFirmware

            FillCapabilities(); */
        }

        public void FillCapabilities()
        {
            string path = "products.json";
            using FileStream openStream = File.OpenRead("products.json");
            JsonNode productsJson = JsonNode.Parse(openStream) ?? throw new InvalidOperationException($"Could not read JSON from file ${path}");

            //foreach (JsonNode vendorJson in productsJson.)
            //{

            //}
            //productsJson["vid"]


            JsonSerializer.Deserialize<Product>(productsJson);
            //local copy of https://github.com/LIFX/products/blob/master/products.json or live?
            //probs local as backup then check for updates
        }
    }
}
