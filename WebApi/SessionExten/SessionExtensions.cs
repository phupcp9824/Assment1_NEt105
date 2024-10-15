using Newtonsoft.Json;

namespace WebApi.SessionExten
{
    public static class SessionExtensions
    {
        // lưu trữ đối tượng dưới json
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value)); // Convert đối tượng thành chuỗi json
        }

        // get
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value); //chuyển đổi chuỗi JSON trở lại thành đối tượng có kiểu T.
        }
    }
}
