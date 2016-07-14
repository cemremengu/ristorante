namespace Ristorante
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class DocumentMessage
    {
        protected JObject json;

        public DocumentMessage()
        {
            json = new JObject();
        }

        public JObject AsJson()
        {
            return JObject.Parse(json.ToString()); // prevent people from fiddling with json object.
        }

        protected virtual void Validate()
        {
        }

        protected T Get<T>(string name) where T : DocumentMessage, new()
        {
            return FromJson<T>((JObject) json[name]);
        }

        protected void Set<T>(string name, T value) where T : DocumentMessage
        {
            json[name] = value.AsJson();
        }

        protected JValue Get(string name)
        {
            return (JValue) json[name];
        }

        protected void Set(string name, JValue value)
        {
            json[name] = value;
        }

        public T To<T>() where T : DocumentMessage, new()
        {
            var result = FromJson<T>(json);

            result.Validate();

            return result;
        }

        public static T FromJson<T>(JObject json) where T : DocumentMessage, new()
        {
            return new T {json = json};
        }

        public override string ToString()
        {
            return json.ToString(Formatting.Indented);
        }
    }
}