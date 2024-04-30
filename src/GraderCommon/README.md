# GraderCommon
## Overview
A collection of classes, enums, ad exceptions used throughout the AutoGrader
## Structure
### Enums
Various enums used throughout GraderCore
### Exceptions
Various exceptions used throughout GraderCore.  
**Note:** any new exceptions should inherit `GraderException` so they can be caught and handled gracefully in the main grading loop
### Reporting
`BatchReport`  
This is the top level report that the main grading loop will return. It contains submission reports for every submission.  

`IncorrectLine`  
Represents a line from a submission that did not match what is expected.    

`SubmissionReport`  
A report on an individual submission containing any incorrect lines, score, and more.  

### SetupInfo
`GradingInfo`   
The base information used to run the grader.  

