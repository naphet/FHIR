using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using System;
using System.IO;

namespace FHIRDataManager.Utility
{
    public static class HL7BaseObjectExtends
    {
        public static String ToJson(this Base baseObject)
        {
            String result = "";
            FhirJsonSerializer ser = new FhirJsonSerializer();
            using(StringWriter textWriter = new StringWriter())
            {
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter);
                var setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                ser.Serialize(baseObject, jsonWriter);
                result =  textWriter.ToString();
            }
            return result;
        }
    }
}
