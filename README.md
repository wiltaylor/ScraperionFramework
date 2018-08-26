# Scraperion Framework
Scraperion is a framework that allows easy UI and Web automation primarily designed for dealing with legacy environments where automation is hard.

The main features of scraperion are as follows:

* UI based automation - Scraperion will find screen elements via reference images. This allows a lot of different scenarios like automation via a VNC/RDP session or Automation of multimedia applications which don't have user controls.

* Web page automation - Using a chromium backend. This allows for the automation of pages which are heavy on javascript or require authentication.

## How to install
To install from PowerShell run the following:
```
install-module scaperion
```

## Examples of screen automation

Loading an image for use.
```
$img = Get-Image -Path ".\Path\To\Image.png"
```

Getting a screenshot
```
$img = Get-Image -Screen
```

Testing if image exists on the screen
```
$img = Get-Image -Path ".\Path\To\Image.png"
if(($img | Test-Image)) { <# Do Something #> }
```
Waiting for an image to appear on screen
```
$img = Get-Image -Path ".\Path\To\Image.png"
$img | Wait-Image
```

Example of clicking on an image on the screen.
```
$img = Get-Image -path ".\Path\To\Image.png"
$img | Select-Image -Click
```

Showing an image preview so you can see what it is
```
$img = Get-Image -path ".\Path\To\Image.png"
$img | Show-Image
```

Extract a string from an image using OCR
```
$img = Get-Image -path ".\Path\To\Image.png"
$text = $img | ConvertFrom-ImageToText
```

Moving the mouse to a location and clicking on it.
```
Move-Mouse -X 100 -Y 100
Send-Mouse -Click
```

Sending keys
```
Send-Keys -Text "hello world"
```

Sending a secure string
```
# $apikey contains a password as a secure string
Send-Keys -SecureText $apikey
```

Sending a ps credential object. This will press tab between username and password.
```
$creds = Get-Credential
Send-Keys -Credential $creds
```

## Examples of web page automation

Connecting to web scraping session
```
$scraper = Connect-WebScraper -Url 'http://mywebsite.com'
```

Disconnecting to web scraping session
```
$scraper | Disconnect-WebScraper
```

Use credentials for web scraping session
```
$creds = Get-Credential
$scraper = Connect-WebScraper -Url 'http://mywebsite.com' -Credential $creds
```

Setting dimensions of web browser used for scraping
```
$scraper = Connect-WebScraper -Url 'http://mywebsite.com' -Width 800 -Height 600
```

Show chromium UI
```
$scraper = Connect-WebScraper -Url 'http://mywebsite.com' -ShowUI
```

Change browser agent string
By default it uses Chrome on windows 10. 
If you need IE or other strings check out http://www.useragentstring.com/pages/useragentstring.php?name=Internet+Explorer
```
$scraper = Connect-WebScraper -Url 'http://mywebsite.com' -Agent 'super awesome string'
```

Take a snapshot of a web page
```
$scraper = Connect-WebScraper -Url 'http://mywebsite.com'
$img = $scraper | Get-WebScraperSnapshot
```

Save snapshot to file
```
$scraper = Connect-WebScraper -Url 'http://mywebsite.com'
$scraper | Get-WebScraperSnapshot -Path "c:\images\myimg.png"
```

Create PDF of web page
```
$scraper = Connect-WebScraper -Url 'http://mywebsite.com'
$scraper | Get-WebScraperSnapshot -Path "c:\pdfs\my.pdf" -PDF
```

Simulating a mouse movement in browser
```
$scraper = Connect-WebScraper -Url 'http://mywebsite.com'
$scraper | Move-WebScraperMouse -X 100 -Y 100
```

Simulate clicking on an element in browser
```
$scraper = Connect-WebScraper -Url 'http://mywebsite.com'
$scraper | Send-WebScraperMouse -Click -Target '#foo'
```

Simulating a tap on an element in the browser
```
$scraper = Connect-WebScraper -Url 'http://mywebsite.com'
$scraper | Send-WebScraperMouse -Click -Target '.foo' -Tap
```

Simulate key presses in browser window.
```
$scraper = Connect-WebScraper -Url 'http://mywebsite.com'
$scraper | Send-WebScraperKeys "Hello world"
```

Invoke javascript expression in browser (like typing it into the console).
```
$scraper = Connect-WebScraper -Url 'http://mywebsite.com'
$tag = $scraper | invoke-WebScraperExpression -Expression "document.querySelector('.foo');"
```

## Scraperion Framework vs Scraperion PowerShell module
The Scraperion Framework is a .net DLL which contains a simple interface to do all the actions lised above. The Scraperion PowerShell 
module consumes the library and exposes the functionality to PowerShell.

Eventually the plan is to release the framework as a nuget package but at the moment you need to either download the PowerShell module or
build it from source.

## Credits
This project makes use of the following Open Source projects:

* [Puppet Sharp](https://www.puppeteersharp.com) - Awesome chromium automation framework for .net. (License: MIT)
* [Tesseract and .NET Wrapper](https://github.com/charlesw/tesseract) - OCR library used to ocr images. (License: Apache 2.0 for Tesseract and MIT for the .net interop library. See project page for details.).
* [Newtonsoft JSON](https://www.newtonsoft.com/json) - JSON parsing library 
* [XmlDoc2CmdletDoc](https://github.com/red-gate/XmlDoc2CmdletDoc) - Awesome tool that converts xml docs in C# into cmdlet doc in your PowerShell module.