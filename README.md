# CalibreCleaner

[Calibre](http://calibre-ebook.com/) is an open-source application for managing your e-book 
library.  Calibre stores all of your books in a directory structure, and uses a 
[SQLite](http://sqlite.org/) database to store metadata about the books in your library.

To access my e-book library across multiple computers, I have tried using Calibre with 
various cloud storage providers (e.g. [Dropbox](https://www.dropbox.com/), 
[OneDrive](https://onedrive.live.com/), [Copy.com](https://www.copy.com/), etc).  Cloud 
storage may be great for photo and documents, but it is not perfect for application files 
that change rapidly as a program runs.  Books sometimes go "missing" in Calibre, because 
its SQLite database gets out of sync with the files in the library directory.

CalibreCleaner is a simple app that I wrote to identify sync problems in need of remedy 
(as well as get some practice with C# and WPF/XAML).  Browse to the root directory of your 
library (i.e. the "Calibre Library" folder), and CalibreCleaner will analyze which books 
are out of sync.  Issues must still be fixed manually, as CalibreCleaner never performs 
any write operations on either the SQLite database or the filesystem.

<img src="https://raw.githubusercontent.com/steve-perkins/CalibreCleaner/master/screenshot.png"/>
