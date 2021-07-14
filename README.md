# QuickLook Bridge

use [windows.protocol] to open QuickLook

```
quick-look://toggle/?file=
```

uwp sample code:

```xml
var param = Uri.EscapeDataString(filePath);
var uri = $"quick-look://toggle/?file={param}";
await Launcher.LaunchUriAsync(new Uri(uri));
```

