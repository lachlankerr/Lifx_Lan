using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan
{
    internal class Features
    {
        public bool hev { get; set; } = false;

        public bool color { get; set; } = false;

        public bool chain { get; set; } = false;

        public bool matrix { get; set; } = false;

        public bool relays { get; set; } = false;

        public bool buttons { get; set; } = false;

        public bool infrared { get; set; } = false;

        public bool multizone { get; set; } = false;

        public int[] temperature_range { get; set; } = new int[2];

        public bool extended_multizone { get; set; } = false;

        public override string ToString()
        {
            return $@"
    HEV: {hev}
    Color: {color}
    Chain: {chain}
    Matrix: {matrix}
    Relays: {relays}
    Buttons: {buttons}
    Infrared: {infrared}
    Multizone: {multizone}
    Temperature Range: {temperature_range[0]} - {temperature_range[1]}
    Extended Multizone: {extended_multizone}";
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Features features = (Features)obj;
                return this.hev == features.hev &&
                       this.color == features.color &&
                       this.chain == features.chain &&
                       this.matrix == features.matrix &&
                       this.relays == features.relays &&
                       this.buttons == features.buttons &&
                       this.infrared == features.infrared &&
                       this.multizone == features.multizone &&
                       this.temperature_range.SequenceEqual(features.temperature_range) && 
                       this.extended_multizone == features.extended_multizone;
            }
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(hev);
            hash.Add(color);
            hash.Add(chain);
            hash.Add(matrix);
            hash.Add(relays);
            hash.Add(buttons);
            hash.Add(infrared);
            hash.Add(multizone);
            hash.Add(temperature_range);
            hash.Add(extended_multizone);
            return hash.ToHashCode();
        }
    }
}
