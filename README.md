# OLS50C RDK Tutorial (C#)
OLS5000 Remote Development Kit (OLS5000 RDK) is equipped with communication functions via sockets, and can control operations of acquisition, measurement, etc. with commands by connecting User PC as the external communication device.

This quickstart tutorial guides you through installing, and running a C# example code to get you started. The sample project is developed based on .NET Framework 4.6 and can be download [here](https://github.com/ospqul/OLS50C_RDK_Demo). The details of all the commands used in the example code can be found in **OLS5000 RDK User's Manual**.

### 1 Installation

Go to software installer folder `OLS50C-RDK_vxxx`, double-click `setup.exe`, and follow the instruction to install in **User PC**. 

**User PC** is the remote PC used to communicate with RDK Server; **Controller PC** is the local PC that runs the RDK Server and controls the OLS5000.

### 2 Network Settings

The User PC communicates with Controller PC via sockets, so they are required to connect into the same network. You can either connect the User PC and the Controller PC with an ethernet cable directly, or connect both to the same network switch.

#### 2.1 Controller PC

The default IP address of the Controller PC is `192.168.0.1`.

#### 2.2 User PC

The User PC's IP address shall be set to a static IP address within the same IP range, for example:

```cs
IP address: 192.168.0.2
Subnet mask: 255.255.255.0
```

![](images/ipv4_settings.png)

You can use the following command in **Command Prompt** to check if the connection is successful.

```bash
ping 192.168.0.1
```

### 3 Use of RDK

> :warning: **WARNING:**
>
> 1. Be aware of the moving parts in the OSL5000.
>
> 2. Make sure X-Y stage has ample space to operate freely.
>
> 3. Make sure the sample not hit by the lens during switching lens or movement of the z axis.

#### 3.1 Initialize

#### 3.1.1 Connect TCP

The RDK Server is running as a TCP Server, and a TCP Client can connect to its port `50100`.

Here is a sample `ClientModel`class to implement the basic functions of a TCP Client:

https://github.com/ospqul/OLS50C_RDK_Demo/blob/main/OLS50C_RDK_Demo/ClientModel.cs

##### Connect()

------

Connect the TCP server with an IP address and port number.

##### Close()

---

Close the TCP connection.

##### Write()

---

Send a text message to TCP Server.

**Receive()**

---

Receive a text message from TCP Server.

#### 3.1.2 Request Connection

**Command:**

`CONNECT= 0`

Description:

Establishes the connection with OLS5000.

#### 3.1.3 Request Normal Start

**Command:** 

`INITNRML= TMELD,olympus`

Description: 

Specifies Login ID and Password to start and initialize OLS5000. Once the RDK Server receives this command, it will launch Data Acquisition Application and Analysis Application in remote mode. 

![](images/aquisition_application.jpg)

![](images/acquisition_login.jpg)

> :warning: **NOTE:**
>
> If the successful completion notification is not returned in 2 minutes, confirm if the macro application is open.
>
> Default location in the Controller PC:
>
> `C:\Program Files\OLYMPUS\LEXT-OLS50-SW\MacroApp\MacroApp\macro.exe`

![](images/macro_application.jpg)

#### 3.2 Functions

#### 3.2.1 Move Stage

**Command:**

`MVSTG= x,y`

Description: 

Moves the motorized XY stage to the specified coordinates.

Move out the stage: `MVSTG= -50000,50000`

Move in the stage: `MVSTG= 0,0`

>:warning: **WARNING:**
>
>Make sure X-Y stage has ample space to operate freely before send this command.

#### 3.2.2 Switch Objective Lens and Change Zoom

**Switch Lens Command:**

`CHOB= OB`

OB = 1, 2, 3, 4, 5, 6

Description: 

Changes to the objective lens specified by rotating the Z Revolver of OLS5000. For example `CHOB= 2` command will switch to the second objective lens.

>  :warning: **WARNING:**
>
> Make sure the lens will not hit the sample on the stage.

**Change Zoom Command:**

`CHZOOM= ZOOM`

ZOOM = 10, 12, 15, 20, 30, 40, 60, 80

Description: 

Changes the zoom magnification of OLS5000 to the specified magnification. For example, `CHZOOM= 30` command will change the zoom to x3.0.

![](images/change_lens_and_zoom.jpg)

#### 3.2.4 Move Home Position

**Command:**

`MVZHOME= 0`

**Description:** 

Moves Z Revolver of OLS5000 to the home position.

> :warning: **WARNING:**
>
> Make sure the lens will not hit the sample on the stage.

#### 3.2.5 Load and Execute Macros

**Load Macros Command:**

`RDWIZ= NAME`

NAME: Macro Names (Either one of return values of `GETWIZNAME?` command)

Description: 

Loads the macros registered in OLS5000. For example, `RDWIZ= macro1.mcr` command will load macro1.mcr.

**Execute Macros Command:**

`WIZEXE= 0`

Description: 

Starts the execution of macros opened in OLS5000. A completion notification is returned after completion of macro.

### 3.3 Disconnect

#### 3.3.1 Switch Modes

**Command:**

`CHMODE= MODE`

MODE = 0 Local Mode

MODE = 1 Remote Mode

Description: 

Switches OLS5000 mode (Remote / Local) from User PC. For example, send `CHMODE= 0` command to switch OLS5000 to local mode before disconnect.

![](images/local_mode.jpg)

#### 3.3.2 Request Disconnection

**Command:**

`DISCONNECT= 0`

Description: 

Disconnects from OLS5000.