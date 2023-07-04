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
        private const string NOT_FOUND = "NOT_FOUND";

        public string Label { get; } = "";

        public int Vendor_ID { get; } = 0;

        public string Vendor_Name { get; set; } = "";

        public int Product_ID { get; } = 0;

        public string Product_Name { get; set; } = "";

        public int Firmware_Major { get; } = 0;

        public int Firmware_Minor { get; } = 0;

        public Features Features { get; set; } = new Features();

        public Product(string label, int vendor_id, int product_id, int firmware_major, int firmware_minor)
        {
            Label = label;                      //GetLabel
            Vendor_ID = vendor_id;              //GetVersion
            Product_ID = product_id;            //GetVersion
            Firmware_Major = firmware_major;    //GetHostFirmware
            Firmware_Minor = firmware_minor;    //GetHostFirmware

            FillCapabilitiesJsonNode();
            //FillCapabilitiesJsonDocument();
        }

        public void FillCapabilitiesJsonNode()
        {
            JsonArray emptyArray = new JsonArray();
            string path = "products.json";
            using FileStream openStream = File.OpenRead(path);
            JsonNode values = JsonNode.Parse(openStream) ?? throw new InvalidOperationException($"Could not read JSON from file ${path}");

            foreach (JsonNode? vendor in values.AsArray())
            {
                if (Vendor_ID == vendor?["vid"]?.GetValue<int>())
                {
                    Vendor_Name = vendor["name"]?.GetValue<string>() ?? "";

                    foreach (JsonNode? product in vendor["products"]?.AsArray() ?? emptyArray)
                    {
                        if (Product_ID == product?["pid"]?.GetValue<int>())
                        {
                            Product_Name = product["name"]?.GetValue<string>() ?? "";

                            JsonNode? features = product["features"];
                            Features = JsonSerializer.Deserialize<Features>(features) ?? new Features();

                            foreach (JsonNode? upgrade in product["upgrades"]?.AsArray() ?? emptyArray)
                            {
                                if (Firmware_Major > upgrade?["major"]?.GetValue<int>() ||
                                    (Firmware_Major == upgrade?["major"]?.GetValue<int>() && Firmware_Minor >= upgrade?["minor"]?.GetValue<int>()))
                                {
                                    JsonNode? newFeatures = upgrade["features"];
                                    Features.hev = newFeatures?["hev"]?.GetValue<bool>() ?? Features.hev;
                                    Features.color = newFeatures?["color"]?.GetValue<bool>() ?? Features.color;
                                    Features.chain = newFeatures?["chain"]?.GetValue<bool>() ?? Features.chain;
                                    Features.matrix = newFeatures?["matrix"]?.GetValue<bool>() ?? Features.matrix;
                                    Features.relays = newFeatures?["relays"]?.GetValue<bool>() ?? Features.relays;
                                    Features.buttons = newFeatures?["buttons"]?.GetValue<bool>() ?? Features.buttons;
                                    Features.infrared = newFeatures?["infrared"]?.GetValue<bool>() ?? Features.infrared;
                                    Features.multizone = newFeatures?["multizone"]?.GetValue<bool>() ?? Features.multizone;
                                    Features.temperature_range = newFeatures?["temperature_range"]?.AsArray().Select(x => x.GetValue<int>()).ToArray() ?? Features.temperature_range;
                                    Features.extended_multizone = newFeatures?["extended_multizone"]?.GetValue<bool>() ?? Features.extended_multizone;
                                }
                            }
                        }
                    }
                }
            }


            //JsonSerializer.Deserialize<Product>(productsJson);
            //local copy of https://github.com/LIFX/products/blob/master/products.json or live?
            //TODO: probs local as backup then check for updates
        }

        public void FillCapabilitiesJsonDocument()
        {
            JsonElement.ArrayEnumerator emptyArray = new JsonElement.ArrayEnumerator();
            string path = "products.json";
            using FileStream openStream = File.OpenRead(path);
            using JsonDocument document = JsonDocument.Parse(openStream);

            JsonElement root = document.RootElement;
            foreach (JsonElement vendor in root.EnumerateArray())
            {
                if (vendor.TryGetProperty("vid", out JsonElement vidElement) && Vendor_ID == vidElement.GetInt32())
                {
                    Vendor_Name = vendor.TryGetProperty("name", out JsonElement nameElement) ? nameElement.GetString() ?? NOT_FOUND : NOT_FOUND;

                    foreach (JsonElement product in vendor.TryGetProperty("products", out JsonElement productsElement) ? productsElement.EnumerateArray() : emptyArray)
                    {
                        if (product.TryGetProperty("pid", out JsonElement pidElement) && Product_ID == pidElement.GetInt32())
                        {
                            Product_Name = product.TryGetProperty("name", out JsonElement pnameElement) ? pnameElement.GetString() ?? NOT_FOUND : NOT_FOUND;

                            if (product.TryGetProperty("features", out JsonElement features))
                            {
                                Features = features.Deserialize<Features>() ?? new Features();
                            }

                            foreach (JsonElement upgrade in product.TryGetProperty("upgrades", out JsonElement upgradesElement) ? upgradesElement.EnumerateArray() : emptyArray)
                            {
                                if (Firmware_Major > (upgrade.TryGetProperty("major", out JsonElement majorElement) ? majorElement.GetInt32() : 0) ||
                                    (Firmware_Major == majorElement.GetInt32() && Firmware_Minor >= (upgrade.TryGetProperty("minor", out JsonElement minorElement) ? minorElement.GetInt32() : 0)))
                                {   
                                    if (upgrade.TryGetProperty("features", out JsonElement ufeatures))
                                    {
                                        Features.hev = ufeatures.TryGetProperty("hev", out JsonElement hevElement) ? hevElement.GetBoolean() : Features.hev;
                                        Features.color = ufeatures.TryGetProperty("color", out JsonElement colorElement) ? colorElement.GetBoolean() : Features.color;
                                        Features.chain = ufeatures.TryGetProperty("chain", out JsonElement chainElement) ? chainElement.GetBoolean() : Features.chain;
                                        Features.matrix = ufeatures.TryGetProperty("matrix", out JsonElement matrixElement) ? matrixElement.GetBoolean() : Features.matrix;
                                        Features.relays = ufeatures.TryGetProperty("relays", out JsonElement relaysElement) ? relaysElement.GetBoolean() : Features.relays;
                                        Features.buttons = ufeatures.TryGetProperty("buttons", out JsonElement buttonsElement) ? buttonsElement.GetBoolean() : Features.buttons;
                                        Features.infrared = ufeatures.TryGetProperty("infrared", out JsonElement infraredElement) ? infraredElement.GetBoolean() : Features.infrared;
                                        Features.multizone = ufeatures.TryGetProperty("multizone", out JsonElement multizoneElement) ? multizoneElement.GetBoolean() : Features.multizone;
                                        Features.temperature_range = ufeatures.TryGetProperty("temperature_range", out JsonElement temperature_rangeElement) ? temperature_rangeElement.EnumerateArray().Select(x => x.GetInt32()).ToArray() : Features.temperature_range;
                                        Features.extended_multizone = ufeatures.TryGetProperty("extended_multizone", out JsonElement extended_multizoneElement) ? extended_multizoneElement.GetBoolean() : Features.extended_multizone;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            return $@"Label: {Label}
Vendor: {Vendor_Name} ({Vendor_ID})
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
                return this.Label == product.Label &&
                       this.Vendor_ID == product.Vendor_ID &&
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
            return HashCode.Combine(Label, Vendor_ID, Vendor_Name, Product_ID, Product_Name, Firmware_Major, Firmware_Minor, Features);
        }
    }
}
