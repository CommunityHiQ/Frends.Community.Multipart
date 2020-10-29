# Frends.Community.MultipartParser

A frends task form parsing multipart/form-data messages. 

[![Actions Status](https://github.com/CommunityHiQ/Frends.Community.MultipartParser/workflows/PackAndPushAfterMerge/badge.svg)](https://github.com/CommunityHiQ/Frends.Community.MultipartParser/actions) ![MyGet](https://img.shields.io/myget/frends-community/v/Frends.Community.MultipartParser) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT) 

- [Installing](#installing)
- [Tasks](#tasks)
     - [MultipartTasks](#MultipartTasks)
- [Building](#building)
- [Contributing](#contributing)
- [Change Log](#change-log)

# Installing

You can install the Task via frends UI Task View or you can find the NuGet package from the following NuGet feed
https://www.myget.org/F/frends-community/api/v3/index.json and in Gallery view in MyGet https://www.myget.org/feed/frends-community/package/nuget/Frends.Community.MultipartParser

# Tasks

## MultipartTasks

A frends task form parsing multipart/form-data messages. Does not use streams, so don't use for very huge files.

### Input

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| ByteArray | `byte[]` | Byte array thta contains multipart message. Usually come from [HttpRequestBytes](https://github.com/FrendsPlatform/Frends.Web#httpsendbytes) task via `#result[Get attachment from Procountor].BodyBytes` or from trigger via `System.Convert.FromBase64String(#trigger.data.httpContentBytesInBase64`). | `foo` |


### Returns

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| Files | List<string Name, byte[] Contents > | List of files in message. | `[{"Name": "employees.json","Contents": "bytes (frends UI shows them as base64 encoded)"},` |
| Parameters | List<string Name, string Value> | Repeated string. | `[{"Name": "username", "Value": "example"}]` |

### Example

Given input 

```----------------------------177392451783996640800119
Content-Disposition: form-data; name=""; filename="employees.json"
Content-Type: application/json

{
    "employee": {
        "name": "Albert Einstein",
        "quote": "Insanity: doing the same thing over and over again and expecting different results."
    }
}
----------------------------177392451783996640800119
Content-Disposition: form-data; name="username"

example
----------------------------177392451783996640800119
Content-Disposition: form-data; name="email"

example@example.com
----------------------------177392451783996640800119
Content-Disposition: form-data; name=""; filename="Lorem Ipsum.pdf"
Content-Type: application/pdf

//lot of binary data from PDF
----------------------------177392451783996640800119--
```

the task would return


```
{
	"Files": [
		{
			"Name": "employees.json",
			"Contents": "bytes (frends UI shows them as base64 encoded)"
		},
		{
			"Name": "Lorem Ipsum.pdf",
			"Contents": "bytes (frends UI shows them as base64 encoded)"
		}
	],
	"Parameters": [
		{
			"Name": "username",
			"Value": "example"
		},
		{
			"Name": "email",
			"Value": "example@example.com"
		}
	]
}
```

Note that also plain text file is returned as byte array.

# Building

Clone a copy of the repository

`git clone https://github.com/CommunityHiQ/Frends.Community.MultipartParser.git`

Rebuild the project

`dotnet build`

Run tests

`dotnet test`

Create a NuGet package

`dotnet pack --configuration Release`

# Contributing
When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repository on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes

NOTE: Be sure to merge the latest from "upstream" before making a pull request!

# Change Log

| Version | Changes |
| ------- | ------- |
| 0.0.1   | Development still going on |
