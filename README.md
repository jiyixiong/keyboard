# Keyboard

PositionMapper.cs 继承自MonoBehaviour

手势检测和控制组件，提供手势当前情况的判断接口，供Manager调用并处理



Manager.cs 继承自MonoBehaviour

逻辑控制组件，负责顶层的逻辑控制，根据左右光标是否进行点击操作来确定当前输入字符并且输出在input框中，检测当前手势并作出处理

包含：左右键盘组件，input框，各种手势标志



KeyboardComponent.cs 继承自MonoBehaviour

记录左右按键应该包含的字符数组



Creator.cs 继承自component

初始化键盘，为左右键盘添加按键数组，初始化按键上的字母，设定键盘边距，添加键盘素材。

包含：左右键盘，左右键盘按键数组，按键素材



KeyboardItem.cs 继承自component

单个按键的组件，记录自身状态和改变按键样式，通过碰撞框来确定当前状态，通过外界的函数调用来修改

包含：isLeft，getclicked，clicked（本次碰撞中已经被点击过），hold（碰撞中）



LeftKeyboard.cs 继承自component

左键盘组件，可以设置通过position设置各个键的值，可以设定各个键的点击状态并根据状态返回键的值

包含：按键数组，output字符串



RightKeyboard.cs 继承自component

左键盘组件，可以设置通过position设置各个键的值，可以设定各个键的点击状态并根据状态返回键的位置

包含：按键数组，output字符串















