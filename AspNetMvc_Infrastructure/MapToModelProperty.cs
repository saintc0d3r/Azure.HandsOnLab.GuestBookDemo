using System;

namespace AspNetMvc_Infrastructure
{
    public class MapToModelProperty : Attribute
    {
        public MapToModelProperty(string modelPropertyName)
        {
            ModelPropertyName = modelPropertyName;
        }

        public string ModelPropertyName { get; set; }
    }
}