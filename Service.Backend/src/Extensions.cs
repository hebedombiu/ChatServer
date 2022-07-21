namespace Service.Backend; 

public static class Extensions {
    public static long ToUnixSeconds(this DateTime dateTime) {
        return ((DateTimeOffset) dateTime).ToUnixTimeSeconds();
    }

    public static long ToUnixMilliseconds(this DateTime dateTime) {
        return ((DateTimeOffset) dateTime).ToUnixTimeMilliseconds();
    }
}