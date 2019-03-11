Welcome to Airport Automation project page
=====

Releases will be available as soon as master is stable. You can still pull from repository and compile it.

### Cloning

For windows, you need [git-scm](https://git-scm.com/). Download an and install from an installer.
To check if git is installed type:

`git --version` to cmd or terminal.

Linux has git by default. However visual studio is not available for linux. You will need to use MonoDevelop to compile instead.
Linux compilation is not tested, but it should work.
To clone repository type:

`git clone https://github.com/jangofett4/AirportAutomation.git` to cmd or terminal.

This should create a new directory called 'AirportAutomation'. Solution files are inside of 'AirportAutomation > AirportAutomation'.

### Compiling

Open the solution with Visual Studio or Mono Develop
Newest code changes are in 'dev' branch, so if you don't see single empty form when visual studio opens, you need to switch to 'dev' branch.
This can be done two ways:

 Use `git checkout dev` in git directory
 
 Use button at bottom right of the visual studio
 
After switching to 'dev' branch use Build menu or Build and Run button to build the application.
Visual Studio will refresh nuget packages automatically on first build.

### Dependencies

 [MySql.Data](https://www.nuget.org/packages/MySql.Data/)
 
 Also [Google.Protobuf](https://www.nuget.org/packages/Google.Protobuf/) which comes with MySql.Data by default.
 
These dependencies are auto-installed upon first build, so there is no need to download manually.
Also for using application you will need a database server.

 [MySql Workbench](https://dev.mysql.com/downloads/workbench/)
 [MySql Server](https://dev.mysql.com/downloads/mysql/)
 
Install both of them. Application needs an empty vessel (an empty database) to run.

 Skeleton database to run on: TODO: add skeleton database, sorry!

### Support

Send me a millon bitcoins.
