# updatecmd升级辅助程序

## **服务端**

**updatecmdsvr**

```
updatecmdsvr start [options]

options:
    --port                  服务端口
    --protocol              服务协议：ftp|tcp|udp|http
    --root                  文件根目录
    --userlist              有效用户列表文件，内容格式一行一个用户：<username>|<group>|<password>
    --whitelist             白名单列表文件，一行一个用户
    --blacklist             黑名单列表文件，一行一个用户
    --enable-userlist       启用用户列表，启用后需登录才能访问
    --enable-whitelist      启用白名单
    --enable-blacklist      启用黑名单

samples:
    updatecmdsvr start --protocol=tcp --port=8090 --root="./publish"
```

## **publish**

发布指令

```
updatecmd publish [options]

options: 
    --name          必须，发布升级包唯一名称
    --version       必须，发布升级包版本
    --files         必须，发布源文件所在文件夹
    --lowest        标志当前版本为最低版本
    --include       包含文件/文件夹筛选配置，';'分割
    --except        排除文件/文件夹筛选配置，';'分割
                    排除优先级高于包含
    --include-conf  包含文件/文件夹筛选配置文件，一行一条
                    优先级: --include > --include-conf
    --except-conf   排除文件/文件夹筛选配置文件，一行一条
                    排除优先级高于包含
                    优先级: --except > --except-conf
    --protocol      服务协议：ftp|tcp|udp|http|file
    --root          文件保存根目录，仅在--protocol=file时可用
    --server        服务地址，仅在--protocol=ftp|tcp|udp|http时可用
    --port          服务端口，仅在--protocol=ftp|tcp|udp|http时可用
    --user          登录账号，需要登录时可用
    --pass          登录密码，需要登录时可用

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
            "url": "/hello/update@1.0.1.1201.json"
        },
        {
            "version": "1.0.1",
            "url": "/hello/update@1.0.1.json"
        },
        {
            "version": "1.0.0",
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

## **download**

下载指令

```
updatecmd download [options]

options:

```

## **pakage**

打包指令


