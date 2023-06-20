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
    }
}
