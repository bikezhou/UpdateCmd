# updatecmd升级辅助程序

## **服务端**

**updatecmdsvr**

```
updatecmdsvr start [options]

options:
    --urls                  服务地址列表
    --root                  文件根目录
    --userlist              有效用户列表文件，内容格式一行一个用户：<username>|<group>|<password>
    --whitelist             白名单列表文件，一行一个用户
    --blacklist             黑名单列表文件，一行一个用户
    --enable-userlist       启用用户列表，启用后需登录才能访问
    --enable-whitelist      启用白名单
    --enable-blacklist      启用黑名单

urls samples:
    file:///c:/hello/update/;http://127.0.0.1:8091/;ftp://127.0.0.1:8092/

samples:
    updatecmdsvr start --protocol=tcp --port=8090 --root="./publish"
```

## **publish**

发布指令

```
updatecmd publish [options]

options: 
    --name          必需，发布升级包名称
    --version       必需，发布升级包版本
    --files         必需，发布源文件所在目录
    --lowest        标志当前版本为最低版本
    --include       包含文件/目录筛选配置，';'分割
    --exclude       排除文件/目录筛选配置，';'分割
                    排除优先级高于包含
    --include-conf  包含文件/目录筛选配置文件，换行符分割
                    优先级: --include > --include-conf
    --exclude-conf  排除文件/目录筛选配置文件，换行符分割
                    排除优先级高于包含
                    优先级: --except > --except-conf
    --url           服务地址url
    --user          登录账号，需要登录时可用
    --pass          登录密码，需要登录时可用

url samples:
    file:///c:/hello/update
    http://127.0.0.1:8091/update
    ftp://127.0.0.1:8091/update

samples:
    updatecmd publish --name=hello --version=1.0.0 --lowest --files="D:\\Hello\\Publish\\" --include="*" --except="obj/;*.pdb"
```

升级列表文件uplist.json

```json
{
    "current":{
        "version": "1.0.1.1201",
        "lowest": "1.0.1",
        "url": "/hello/update.json"
    },
    "uplist": [
        {
            "version": "1.0.1.1201",
            "lowest": "1.0.1",
            "url": "/hello/update@1.0.1.1201.json"
        },
        {
            "version": "1.0.1",
            "lowest": "1.0.1",
            "url": "/hello/update@1.0.1.json"
        },
        {
            "version": "1.0.0",
            "lowest": "1.0.0",
            "url": "/hello/update@1.0.0.json"
        }
    ]
}
```

升级文件update.json

```json
{
    "version": "1.0.0",
    "lowest": "1.0.0",
    "files": [
        {
            "name": "hello.exe",
            "url": "/hello/1.0.0/files/hello.exe",
            "md5": ""
        },{
            "name": "",
            "url": "/hello/1.0.0/files/config/hello.config",
            "md5": ""
        }
    ]
}
```
版本文件列表upfiles.json

```json
{
    "version": "1.0.0",
    "lowest": "1.0.0",
    "files": [
        {
            "name": "hello.exe",
            "url": "/hello/1.0.0/files/hello.exe",
            "md5": ""
        },{
            "name": "",
            "url": "/hello/1.0.0/files/config/hello.config",
            "md5": ""
        }
    ]
}
```

*发布目录结构*

```
update/
    |-hello/
        |-1.0.0/
            |-files/
                |-hello.exe
                |-...
            |-upfiles.json
        |-1.0.1/
            |-files/
                |-hello.exe
                |-...
            |-upfiles.json
        |-update.json
        |-update@1.0.0.json
        |-update@1.0.1.json
        |-uplist.json
```

## **download**

下载指令

```
updatecmd download [options]

options:
    --name      必需，已发布升级包名称
    --url       服务url地址
    --version   下载版本
    --output    文件下载目录，默认为程序目录下的download/目录下

url samples:
    file:///D:/hello/update/

samples:
    updatecmd download --name=hello --url=file:///D:/hello/update --version=1.0.0 --output="./download"
```

下载文件目录结构：

```
download/
    |-$name/
        |-hello.exe
    |-update.$name.json
```


## **pakage**

打包指令


