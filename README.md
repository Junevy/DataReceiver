# DataReceiver 
> 随手立的一个练习项目，名字有一些随意...原本只是想写个简单的通讯软件。 <br><br>
> 开始写通讯的时候，就在思考是否应该写个简单的导航框架，于是，这个项目便诞生了！ <br><br>
> 导航框架写完后，在框架内写了个FTP Server（基于FubarDev）和TCP Client。 <br><br>

** 项目基于.Net 8，编译Target 为 .Net framework 4.7.2 ** 

## 已实现的功能：
1. TCP Client：包含基本通讯、通讯日志、心跳、重连功能； <br><br>
<img width="1350" height="732" alt="image" src="https://github.com/user-attachments/assets/5fda739c-b0ff-44e7-a314-c913f82c27cb" /> <br><br>
2. FTP Server：基于FubarDev，使用公共用户验证（可在UserShip中管理、维护用户），实现简单的文件管理。同时，因个人需要，又额外添加了一个定时清理文件的功能（注册Windows的TaskSchedule）； <br> <br>
<img width="1500" height="813" alt="image" src="https://github.com/user-attachments/assets/81661573-f1d1-4566-9623-c3c359011bf5" /> <br><br>
3. 已配置好Log4Net，记录程序日志；
   
## 未来会做的事情：
1. Modbus 通讯，对某些硬件进行数据采集； 
2. 各种型号的PLC 通讯协议； 
3. Halcon、海康VM、海康相机、大华（华睿）MVP、YOLO二次开发； 
4. 多文化（多语言）切换（可能很久远！）； 
5. 主题热更换（毕竟最近暗黑模式很流行嘛！）； 

## 使用的包（不含全部）： 
1. Microsoft.Extensions.*; 
2. CommunityToolkit.Mvvm; 
3. System.Reactive; 
4. HandyControl; 
5. Microsoft.Xaml.Behaviors.Wpf; 
6. FubarDev.FTPServer; 




