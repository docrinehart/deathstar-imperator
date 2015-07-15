$script:project_config = "Debug"
properties {
	Framework '4.5.1'

	$majorVersion = 0
	$minorVersion = 1
	$versionType = 1 #version type 0 = build server, 1 = local build
	
	$project_name = "DeathStarImperator"
	$buildCounter = 1
	if(-not $version)
	{
		if(Test-Path .\localbuildcounter.vars.ps1) {
			. .\localbuildcounter.vars.ps1
		}
		$buildCounter = $buildCounter + 1;
		update-buildcounter "$buildCounter" "localbuildcounter.vars.ps1"
		$version = "$majorVersion.$minorVersion.$versionType.$buildCounter"
	}
	
	$ReleaseNumber =  $version

	Write-Host "================================================="
	Write-Host "Release Number: $ReleaseNumber"
	Write-Host "================================================="
	
	$base_dir = resolve-path .
	$build_dir = "$base_dir\build"
	$source_dir = "$base_dir\src"
	$test_dir = "$build_dir\test"
	$result_dir = "$build_dir\results"
	
	$test_assembly_patterns_unit = @("*Tests.dll")

	if(Test-Path .\localbuild.vars.ps1) {
		Write-Host "Detected local path variable overrides. Loading localbuild.vars.ps1"  -foregroundcolor Green;
		. .\localbuild.vars.ps1
	} else {
		Write-Host "Minor Warning: " -foregroundcolor Yellow -nonewline;
		Write-Host "No local path variable overrides, but this is probably what you want. If you need to change the location of a variable such as  'google_drive_project_root' to better fit your local machine configuration you should create a file called 'localbuild.vars.ps1' and set these values there";
	}
	
}

#These are aliases for other build tasks. They typically are named after the camelcase letters (rad = Rebuild All Databases)
#aliases should be all lowercase, conventionally
#please list all aliases in the help task
task default -depends InitialPrivateBuild
task dev -depends DeveloperBuild
task release -depends ReleaseBuild

task help {
	Write-Help-Header
	Write-Help-Section-Header "Comprehensive Building"
	Write-Help-For-Alias "(default)" "Intended for first build or when you want a fresh, clean local copy"
	Write-Help-For-Alias "dev" "Optimized for local dev; Most noteably UPDATES databases instead of REBUILDING"
	Write-Help-For-Alias "release" "Release build with packaging"
	
	Write-Help-Footer
	exit 0
}

#These are the actual build tasks. They should be Pascal case by convention
task InitialPrivateBuild -depends Clean, CommonAssemblyInfo, Compile, RunAllTests

task DeveloperBuild -depends Clean, CommonAssemblyInfo, Compile, RunAllTests

task ReleaseBuild -depends SetReleaseBuild, Clean, CommonAssemblyInfo, Compile

task SetReleaseBuild {
	$script:project_config = "Release"
}

task RestoreNuget {
	exec { & src\.nuget\nuget.exe restore .\src\DeathStarImperator.sln -OutputDirectory src\packages }
}

task CopyAssembliesForTest -Depends Compile {
    copy_all_assemblies_for_test $test_dir
}

task RunAllTests -Depends CopyAssembliesForTest {
    $test_assembly_patterns_unit | %{ run_fixie_tests $_ }
}

task CommonAssemblyInfo {
	create-commonAssemblyInfo "$version" "DeathStar Imperator" "$source_dir\CommonAssemblyInfo.cs"
}

task Compile -depends Clean { 
	exec { msbuild.exe /t:build /v:q /p:Configuration=$project_config /nologo $source_dir\$project_name.sln }
}

task Clean -depends RestoreNuget {
	delete_directory $build_dir

	create_directory $test_dir 
	create_directory $result_dir

	exec { msbuild /t:clean /v:q /p:Configuration=$project_config $source_dir\$project_name.sln }
}

# -------------------------------------------------------------------------------------------------------------
# helper tasks for warnings, etc, that don't really do any work
# -------------------------------------------------------------------------------------------------------------
task WarnSlowBuild {
	Write-Host ""
	Write-Host "Warning: " -foregroundcolor Yellow -nonewline;
	Write-Host "The default build you just ran is primarily intended for initial "
	Write-Host "environment setup. While developing you most likely want the quicker dev"
	Write-Host "build task. For a full list of common build tasks, run: "
	Write-Host " > build.bat help"
}

task EnsurePowershellVersion {
	$ver = [System.Double]::Parse($Host.Version) 
	
	if ($ver -lt 3) {
		Write-Host "*** Failure ***" -foregroundcolor White -backgroundcolor DarkRed;

		Write-Host "You need to upgrade to version 3 or later of powershell for this build script to work properly."  -foregroundcolor White -backgroundcolor DarkRed;
		Write-Host "You will get an error about credentials not supported when trying to restore the database."  -foregroundcolor White -backgroundcolor DarkRed;
		exit(1);

		} else {
		Write-Host "Powershell Version 3.0 or later detected, continuing build" -foregroundcolor Green
	}
}


# -------------------------------------------------------------------------------------------------------------
# helper functions  specific to this project
# -------------------------------------------------------------------------------------------------------------
# N/A

# -------------------------------------------------------------------------------------------------------------
# generalized functions added by Headspring for Help Section
# --------------------------------------------------------------------------------------------------------------

function Write-Help-Header($description) {
	Write-Host ""
	Write-Host "********************************" -foregroundcolor DarkGreen -nonewline;
	Write-Host " HELP " -foregroundcolor Green  -nonewline; 
	Write-Host "********************************"  -foregroundcolor DarkGreen
	Write-Host ""
	Write-Host "This build script has the following common build " -nonewline;
	Write-Host "task " -foregroundcolor Green -nonewline;
	Write-Host "aliases set up:"
}

function Write-Help-Footer($description) {
	Write-Host ""
	Write-Host " For a complete list of build tasks, view default.ps1."
	Write-Host ""
	Write-Host "**********************************************************************" -foregroundcolor DarkGreen
}

function Write-Help-Section-Header($description) {
	Write-Host ""
	Write-Host " $description" -foregroundcolor DarkGreen
}

function Write-Help-For-Alias($alias,$description) {
	Write-Host "  > " -nonewline;
	Write-Host "$alias" -foregroundcolor Green -nonewline; 
	Write-Host " = " -nonewline; 
	Write-Host "$description"
}

# -------------------------------------------------------------------------------------------------------------
# generalized functions 
# --------------------------------------------------------------------------------------------------------------

function run_fixie_tests([string]$pattern) {
    $items = Get-ChildItem -Path $test_dir $pattern
    $items | %{ run_fixie $_.Name }
}

function global:zip_directory($directory,$file) {
	write-host "Zipping folder: " $directory
	delete_file $file
	cd $directory
	& "$base_dir\lib\7zip\7za.exe" a -mx=9 -r $file | Out-Null
	cd $base_dir
}

function global:delete_file($file) {
	if($file) { remove-item $file -force -ErrorAction SilentlyContinue | out-null } 
}

function global:delete_directory($directory_name) {
  rd $directory_name -recurse -force  -ErrorAction SilentlyContinue | out-null
}

function global:create_directory($directory_name) {
  mkdir $directory_name  -ErrorAction SilentlyContinue  | out-null
}

function global:run_fixie ($test_assembly) {
	$assembly_to_test = $test_dir + "\" + $test_assembly
	$results_output = $result_dir + "\" + $test_assembly + ".xml"
	write-host "Running Fixie Tests in: " $test_assembly
	exec { & tools\fixie\Fixie.Console.exe $assembly_to_test --NUnitXml $results_output --TeamCity off }
}

function global:Copy_and_flatten ($source,$include,$dest) {
	ls $source -include $include -r | cp -dest $dest
}

function global:copy_all_assemblies_for_test($destination){
	$bin_dir_match_pattern = "$source_dir\*\bin\$project_config"
	create_directory $destination
	Copy_and_flatten $bin_dir_match_pattern *.exe $destination
	Copy_and_flatten $bin_dir_match_pattern *.dll $destination
	Copy_and_flatten $bin_dir_match_pattern *.config $destination
	Copy_and_flatten $bin_dir_match_pattern *.pdb $destination
	Copy_and_flatten $bin_dir_match_pattern *.sql $destination
	Copy_and_flatten $bin_dir_match_pattern *.xlsx $destination
}

function global:copy_website_files($source,$destination){
	$exclude = @('*.user','*.dtd','*.tt','*.cs','*.csproj') 
	copy_files $source $destination $exclude
	delete_directory "$destination\obj"
}

function global:copy_files($source,$destination,$exclude=@()){    
	create_directory $destination
	Get-ChildItem $source -Recurse -Exclude $exclude | Copy-Item -Destination {Join-Path $destination $_.FullName.Substring($source.length)} 
}

function global:Convert-WithXslt($originalXmlFilePath, $xslFilePath, $outputFilePath) {
   ## Simplistic error handling
   $xslFilePath = resolve-path $xslFilePath
   if( -not (test-path $xslFilePath) ) { throw "Can't find the XSL file" } 
   $originalXmlFilePath = resolve-path $originalXmlFilePath
   if( -not (test-path $originalXmlFilePath) ) { throw "Can't find the XML file" } 
   #$outputFilePath = resolve-path $outputFilePath -ErrorAction SilentlyContinue 
   if( -not (test-path (split-path $originalXmlFilePath)) ) { throw "Can't find the output folder" } 

   ## Get an XSL Transform object (try for the new .Net 3.5 version first)
   $EAP = $ErrorActionPreference
   $ErrorActionPreference = "SilentlyContinue"
   $script:xslt = new-object system.xml.xsl.xslcompiledtransform
   trap [System.Management.Automation.PSArgumentException] 
   {  # no 3.5, use the slower 2.0 one
	  $ErrorActionPreference = $EAP
	  $script:xslt = new-object system.xml.xsl.xsltransform
   }
   $ErrorActionPreference = $EAP
   
   ## load xslt file
   $xslt.load( $xslFilePath )
	 
   ## transform 
   $xslt.Transform( $originalXmlFilePath, $outputFilePath )
}

function global:create-commonAssemblyInfo($version,$applicationName,$filename) {
	"using System.Reflection;
using System.Runtime.InteropServices;

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: ComVisibleAttribute(false)]
[assembly: AssemblyVersionAttribute(""$version"")]
[assembly: AssemblyFileVersionAttribute(""$version"")]
[assembly: AssemblyCopyrightAttribute(""Copyright 2015"")]
[assembly: AssemblyProductAttribute(""$applicationName"")]
[assembly: AssemblyCompanyAttribute(""Doc Rinehart"")]
[assembly: AssemblyConfigurationAttribute(""release"")]
[assembly: AssemblyInformationalVersionAttribute(""$version"")]"  | out-file $filename -encoding "ASCII"    
}

function global:create-buildInfo($version,$applicationName,$filename) {
	"<?xml version=""1.0"" ?>
<buildInfo>
  <projectName>$applicationName</projectName>
  <companyName>Doc Rinehart</companyName>
  <version>$version</version>
</buildInfo>"  | out-file $filename -encoding "ASCII"    
}

function global:update-buildcounter($version,$filename) {
	"`$buildCounter = $version"  | out-file $filename -encoding "ASCII"    
}



function script:poke-xml($filePath, $xpath, $value, $namespaces = @{}) {
	[xml] $fileXml = Get-Content $filePath
	
	if($namespaces -ne $null -and $namespaces.Count -gt 0) {
		$ns = New-Object Xml.XmlNamespaceManager $fileXml.NameTable
		$namespaces.GetEnumerator() | %{ $ns.AddNamespace($_.Key,$_.Value) }
		$node = $fileXml.SelectSingleNode($xpath,$ns)
	} else {
		$node = $fileXml.SelectSingleNode($xpath)
	}
	
	Assert ($node -ne $null) "could not find node @ $xpath"
		
	if($node.NodeType -eq "Element") {
		$node.InnerText = $value
	} else {
		$node.Value = $value
	}

	$fileXml.Save($filePath) 
}

function usingx {
	param (
		$inputObject = $(throw "The parameter -inputObject is required."),
		[ScriptBlock] $scriptBlock
	)

	if ($inputObject -is [string]) {
		if (Test-Path $inputObject) {
			[void][system.reflection.assembly]::LoadFrom($inputObject)
		} elseif($null -ne (
			  new-object System.Reflection.AssemblyName($inputObject)
			  ).GetPublicKeyToken()) {
			[void][system.reflection.assembly]::Load($inputObject)
		} else {
			[void][system.reflection.assembly]::LoadWithPartialName($inputObject)
		}
	} elseif ($inputObject -is [System.IDisposable] -and $scriptBlock -ne $null) {
		Try {
			&$scriptBlock
		} Finally {
			if ($inputObject -ne $null) {
				$inputObject.Dispose()
			}
			Get-Variable -scope script |
				Where-Object {
					[object]::ReferenceEquals($_.Value.PSBase, $inputObject.PSBase)
				} |
				Foreach-Object {
					Remove-Variable $_.Name -scope script
				}
		}
	} else {
		$inputObject
	}
}