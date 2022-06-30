Add your API key to the constructor of partial class APIKey/APIKeyLocal.cs:

__APIKey/APIKeyLocal.cs__

```
public static partial class APIKey
{
    static APIKey()
    {
        BingMapsAPIKey = "YOURAPIKEY";
    }
}
```