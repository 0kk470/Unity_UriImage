# Unity_UriImage
This plugin makes ugui's image support displaying sprite from url, which is based on [UnityWebRequest](https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequestTexture.GetTexture.html)

### Installation
***
#### Use UPM
#### Dowanload unitypackage


### Usage
***
#### Create a new UriImage
![sc1](https://user-images.githubusercontent.com/25216715/169456106-ea284351-0722-48ac-88e2-fa6e5658a66e.gif)
#### Configure URL on unity inspector
* ##### Load sprite from local disk
![sc2](https://user-images.githubusercontent.com/25216715/169456991-f832ab00-b18c-4f74-a455-07255de9649d.gif)
* ##### Load sprite from remote
![sc3](https://user-images.githubusercontent.com/25216715/169456991-f832ab00-b18c-4f74-a455-07255de9649d.gif) 
#### Load Sprite via code
```Csharp
using SaltyfishKK.UriImage;

public class YourClassOrSomethingElse:MonoBehaviour
{
    [SerializeField]
    private UriImage m_Img;
    
    void DoSomething()
    {
        m_Img.LoadSpriteFromUri("url", UriSourceType.Remote);
    }
}
```
#### Use default Image
Only want use UGUI Image? Then just call these functions.
```Csharp
UriSpriteLoader.Instance.DisplayFromRemote(ImgComponent, "url");
UriSpriteLoader.Instance.DisplayFromFilePath(ImgComponent, "url");
```
#### Custom your own ErrorImage
1.Find the <b>UriImage_Setting</b> scriptable asset.

2.Modify the <b>DefaultErrorImagePath</b> (relative to Resources Folder).

3.Put your own sprite asset in the specific folder.
![uriimage_setting](https://user-images.githubusercontent.com/25216715/169448552-472b09e8-8b83-4015-bf72-b4f24af34c18.png)
