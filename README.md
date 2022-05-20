# Unity_UriImage
This plugin makes ugui's image support displaying sprite from url, which is based on [UnityWebRequest](https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequestTexture.GetTexture.html)

## Installation
### I. Use UPM
* Open your package manager window and select "add package from git URL".
![img_upm](https://user-images.githubusercontent.com/25216715/169458905-3554ec54-2f28-4c02-85cf-8f05dd40d068.png)
* Type in "https://github.com/0kk470/Unity_UriImage.git#upm"

### II. Dowanload unitypackage
Or download the [unitypackage](https://github.com/0kk470/Unity_UriImage/raw/master/UriImage.unitypackage) and import.

## Usage

### Create a new UriImage
![sc1](https://user-images.githubusercontent.com/25216715/169456106-ea284351-0722-48ac-88e2-fa6e5658a66e.gif)
### Configure URL on unity inspector
 #### 1. Load sprite from local disk
 ![sc2](https://user-images.githubusercontent.com/25216715/169456991-f832ab00-b18c-4f74-a455-07255de9649d.gif)
 #### 2. Load sprite from remote
 ![sc3](https://user-images.githubusercontent.com/25216715/169458886-8a99abf8-b5c0-4789-beaf-1975f737a455.gif) 
### Load Sprite in other script
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
### Use default Image
Only want to use UGUI Image? Then just call these functions.
```Csharp
UriSpriteLoader.Instance.DisplayFromRemote(ImgComponent, "url");
UriSpriteLoader.Instance.DisplayFromFilePath(ImgComponent, "url");
```
### Custom your own ErrorImage
1. Find the <b>UriImage_Setting</b> scriptable asset.

2. Modify the <b>DefaultErrorImagePath</b> (relative to Resources Folder).

3. Put your own sprite asset in the specific folder.

![uriimage_setting](https://user-images.githubusercontent.com/25216715/169448552-472b09e8-8b83-4015-bf72-b4f24af34c18.png)
