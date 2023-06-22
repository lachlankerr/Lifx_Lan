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

        public string Vendor_Name { get; set; } = "";

        public int Product_ID { get; } = 0;

        public string Product_Name { get; set; } = "";

        public int Firmware_Major { get; } = 0;

        public int Firmware_Minor { get; } = 0;

        public Features Features { get; set; } = new Features();

        public Product(int vendor_id, int product_id, int firmware_major, int firmware_minor)
        {
            Vendor_ID = vendor_id;              //GetVersion
            Product_ID = product_id;            //GetVersion
            Firmware_Major = firmware_major;    //GetHostFirmware
            Firmware_Minor = firmware_minor;    //GetHostFirmware

            FillCapabilities();
        }

        public void FillCapabilities()
        {
            string path = "products.json";
            using FileStream openStream = File.OpenRead("products.json");
            JsonNode values = JsonNode.Parse(openStream) ?? throw new InvalidOperationException($"Could not read JSON from file ${path}");

            foreach (JsonNode? vendor in values.AsArray())
            {
                if (Vendor_ID == vendor?["vid"]?.GetValue<int>())
                {
                    Vendor_Name = vendor["name"]?.GetValue<string>() ?? "";

                    foreach (JsonNode? product in vendor["products"]?.AsArray()!)
                    {
                        if (Product_ID == product?["pid"]?.GetValue<int>())
                        {
                            Product_Name = product["name"]?.GetValue<string>() ?? "";

                            JsonNode? features = product["features"];
                            Features = JsonSerializer.Deserialize<Features>(features) ?? new Features();

                            foreach (JsonNode? upgrade in product["upgrades"]?.AsArray()!)
                            {
                                if (Firmware_Major > upgrade?["major"]?.GetValue<int>() ||
                                    (Firmware_Major == upgrade?["major"]?.GetValue<int>() && Firmware_Minor >= upgrade?["minor"]?.GetValue<int>()))
                                {
                                    JsonNode? newFeatures = upgrade["features"];
                                    Features = JsonSerializer.Deserialize<Features>(newFeatures) ?? new Features();
                                }
                            }
                        }
                    }
                }
            }


            //JsonSerializer.Deserialize<Product>(productsJson);
            //local copy of https://github.com/LIFX/products/blob/master/products.json or live?
            //probs local as backup then check for updates
        }

        public override string ToString()
        {
            return $@"Vendor: {Vendor_Name} ({Vendor_ID})
Name: {Product_Name} ({Product_ID})
Firmware: ({Firmware_Major}, {Firmware_Minor})
Features: [{Features}
]";
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Product product = (Product)obj;
                return this.Vendor_ID == product.Vendor_ID &&
                       this.Vendor_Name == product.Vendor_Name &&
                       this.Product_ID == product.Product_ID &&
                       this.Product_Name == product.Product_Name &&
                       this.Firmware_Major == product.Firmware_Major && 
                       this.Firmware_Minor == product.Firmware_Minor &&
                       this.Features.Equals(product.Features);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Vendor_ID, Vendor_Name, Product_ID, Product_Name, Firmware_Major, Firmware_Minor, Features);
        }
    }
}
