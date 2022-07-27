using ListServiceProxy.Models;
using System.Collections.Generic;

namespace ListServiceProxy.ExtendedModels
{
    public static class KeyValPairModelMapper
    {
        public static List<KeyValPairShortModelCustom> CustomizeShortKeyPair(this List<dynamic> data)
        {
            List<KeyValPairShortModelCustom> modeltrimmedlist = new List<KeyValPairShortModelCustom>();
            foreach (var main in data)
            {
                KeyValPairShortModelCustom modeltrimmed = new KeyValPairShortModelCustom
                {
                    value = main.Key,
                    label = main.Value
                };
                modeltrimmedlist.Add(modeltrimmed);
            }
            return modeltrimmedlist;
        }


        public static List<KeyValPairModelCustom> CustomizeStringKeyPair(this List<dynamic> data)
        {
            List<KeyValPairModelCustom> modeltrimmedlist = new List<KeyValPairModelCustom>();
            foreach (var main in data)
            {
                KeyValPairModelCustom modeltrimmed = new KeyValPairModelCustom
                {
                    value = main.Key,
                    label = main.Value
                };
                modeltrimmedlist.Add(modeltrimmed);
            }
            return modeltrimmedlist;
        }

        public static List<KeyValPairIntModelCustom> CustomizeIntKeyPair(this List<dynamic> data)
        {
            List<KeyValPairIntModelCustom> modeltrimmedlist = new List<KeyValPairIntModelCustom>();
            foreach (var main in data)
            {
                KeyValPairIntModelCustom modeltrimmed = new KeyValPairIntModelCustom
                {
                    value = main.Key,
                    label = main.Value
                };
                modeltrimmedlist.Add(modeltrimmed);
            }
            return modeltrimmedlist;
        }

        public static List<KeyValPairGuidModelCustom> CustomizeGuidKeyPair(this List<dynamic> data)
        {
            List<KeyValPairGuidModelCustom> modeltrimmedlist = new List<KeyValPairGuidModelCustom>();
            foreach (var main in data)
            {
                KeyValPairGuidModelCustom modeltrimmed = new KeyValPairGuidModelCustom
                {
                    value = main.Key,
                    label = main.Value
                };
                modeltrimmedlist.Add(modeltrimmed);
            }
            return modeltrimmedlist;
        }

        public static List<KeyValPairGuidWithAdditionalModelCustom> CustomizeGuidKeyPairAdditional(this List<dynamic> data)
        {
            List<KeyValPairGuidWithAdditionalModelCustom> modeltrimmedlist = new List<KeyValPairGuidWithAdditionalModelCustom>();
            foreach (var main in data)
            {
                KeyValPairGuidWithAdditionalModelCustom modeltrimmed = new KeyValPairGuidWithAdditionalModelCustom
                {
                    value = main.Key,
                    label = main.Value,
                    mappingKey=main.MappingKey,
                    filler1=main.Filler1
                };
                modeltrimmedlist.Add(modeltrimmed);
            }
            return modeltrimmedlist;
        }
    }
}
