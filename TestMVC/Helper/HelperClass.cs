using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TestMVC.Models;

namespace TestMVC.Helper
{
    public class HelperClass
    {
        //public void AddtoJson(BookModel book)
        //{
        //    var appSettingsPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "appsettings.json");
        //    var json = File.ReadAllText(appSettingsPath);

        //    var jsonSettings = new JsonSerializerSettings();
        //    jsonSettings.Converters.Add(new ExpandoObjectConverter());
        //    jsonSettings.Converters.Add(new StringEnumConverter());

        //    dynamic config = JsonConvert.DeserializeObject<ExpandoObject>(json, jsonSettings);

        //    dynamic newBook = JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(book), jsonSettings);

        //    var expando = config as IDictionary<string, object>;
        //    expando.Add($"Book{book.Id}", newBook);

        //    var newJson = JsonConvert.SerializeObject(config, Formatting.Indented, jsonSettings);

        //    File.WriteAllText(appSettingsPath, newJson);
        //}

        //public void AddtoJson(CompanyModel company)
        //{
        //    var appSettingsPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "appsettings.json");
        //    var json = File.ReadAllText(appSettingsPath);

        //    var jsonSettings = new JsonSerializerSettings();
        //    jsonSettings.Converters.Add(new ExpandoObjectConverter());
        //    jsonSettings.Converters.Add(new StringEnumConverter());

        //    dynamic config = JsonConvert.DeserializeObject<ExpandoObject>(json, jsonSettings);

        //    dynamic newCompany = JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(company), jsonSettings);

        //    var expando = config as IDictionary<string, object>;
        //    expando.Add($"Company{company.Id}", newCompany);

        //    var newJson = JsonConvert.SerializeObject(config, Formatting.Indented, jsonSettings);

        //    File.WriteAllText(appSettingsPath, newJson);
        //}

        // Generic Method
        public void AddtoJson<Object>(Object T)
        {
            var appSettingsPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "appsettings.json");
            var json = File.ReadAllText(appSettingsPath);

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Converters.Add(new ExpandoObjectConverter());
            jsonSettings.Converters.Add(new StringEnumConverter());

            dynamic config = JsonConvert.DeserializeObject<ExpandoObject>(json, jsonSettings);

            dynamic newinput = JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(T), jsonSettings);

            Type t = T.GetType();
            PropertyInfo[] props = t.GetProperties();
            
            var expando = config as IDictionary<string, object>;
            expando.Add($"{t.Name.Remove(t.Name.Length - 5)}{props[0].GetValue(T)}", newinput);

            var newJson = JsonConvert.SerializeObject(config, Formatting.Indented, jsonSettings);

            File.WriteAllText(appSettingsPath, newJson);
        }

        public void RemoveFromJson(int Id,string type)
        {
            var appSettingsPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "appsettings.json");
            var json = File.ReadAllText(appSettingsPath);

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Converters.Add(new ExpandoObjectConverter());
            jsonSettings.Converters.Add(new StringEnumConverter());

            dynamic config = JsonConvert.DeserializeObject<ExpandoObject>(json, jsonSettings);

            var expando = config as IDictionary<string, object>;
            expando.Remove($"{type}{Id}");

            var newJson = JsonConvert.SerializeObject(config, Formatting.Indented, jsonSettings);

            File.WriteAllText(appSettingsPath, newJson);
        }
    }
}
