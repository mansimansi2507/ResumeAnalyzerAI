#!/bin/bash
set -e
cd ResumeAnalyzerAI
dotnet publish -c Release -o out
cd out
dotnet ResumeAnalyzerAI.dll
