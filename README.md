# Job Tracking App 

This web application assists with tracking job searches by allowing user(s) to manage multiple users with job profiles and their related employers all in one place. Users can upload their own Excel or CSV list and also download your list.

## Description

It's always interesting and a little fun to know the history of the hows, whats, and whys on why we did the things we did, right?

This web application started as a way for me to track my job search after being laid off from my 1st job after graduation. I was collecting unemployment at the time which required me to prove I was job hunting. 
It quickly became a chore to track the multiple jobs I applied for. So, I took the skills I learned at my previous jobs, internships etc., and put them to use.

This simple job tracking tool can be broken down into 3 steps.

USERS, JOBS, and EMPLOYERS.

There's an autheticanation step in case you want to host this web application. Register a new user, then log in, a token is utilized here with middleware on the backend checking if you've been authenticated to use the API.
Once a user has logged in, they can create job profiles, and each profile contains employers related to that job profile. Why? You might use this web application later on layoff number 2!!! 
Each employer profile has the details we need to keep track of, like the name, address, emails, websites, and the results and actions you took for this employer.

As a CRUD web application, you can create, update, delete etc. as usual. This web application calls the API when you do so. The job tracking tool also takes into account that maybe you've started your search on Excel or some 
CSV form before using the application, so there's a way to upload your existing data. Along with this, you can download a new list if you ever need to send your information to unemployment!

## Getting Started

### Dependencies

* C#
* .Net 9.0
* Angular: 18.2.10
* Node: 20.17.0
* These nuget packages are used in my project.
1.	AutoMapper - Version 13.0.1
2.	Microsoft.AspNetCore.Authentication.JwtBearer - Version 9.0.1
3.	Microsoft.AspNetCore.Cors - Version 2.3.0
4.	Microsoft.Data.SqlClient - Version 6.0.1
5.	Microsoft.EntityFrameworkCore.Design - Version 9.0.1
6.	Microsoft.Extensions.Configuration.Binder - Version 9.0.1
7.	Swashbuckle.AspNetCore - Version 7.2.0
8.	Swashbuckle.AspNetCore.SwaggerUI - Version 7.2.0
* ex. Windows 10

### Installing

* https://github.com/JaiXiong/JobTrackerTool, this is how you can get my code for this web application

### Executing program

* How to run the program
* * If you're running this locally, make sure you run the APIs and have chosen a hosting option, whether IIS, serve,r or debugging.
* Step-by-step bullets
* * Most of the commands can be found in the package.json
  * For example...
  * ng build
  * ng serve
  * etc.
```
code blocks for commands
```

## Help

Any advise for common problems or issues. TODO, some FAQ here.
```
command to run if program contains helper info
```

## Authors

Jai Xiong, open to inquiries at [email TBD]

ex. Dominique Pizzie  
ex. [@DomPizzie](https://twitter.com/dompizzie)

## Version History

* 0.2
    * Various bug fixes and optimizations
    * See [commit change]() or See [release history]()
* 0.1
    * Initial Release

## License

This project is licensed under the [NAME HERE] License - see the LICENSE.md file for details

## Acknowledgments

Inspiration, code snippets, etc.
* [awesome-readme](https://github.com/matiassingers/awesome-readme)
* [PurpleBooth](https://gist.github.com/PurpleBooth/109311bb0361f32d87a2)
* [dbader](https://github.com/dbader/readme-template)
* [zenorocha](https://gist.github.com/zenorocha/4526327)
* [fvcproductions](https://gist.github.com/fvcproductions/1bfc2d4aecb01a834b46)
