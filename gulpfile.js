var gulp = require('gulp');
var shell = require("gulp-shell");
var fs = require("fs");
var args = require('yargs').argv;
var assemblyInfo = require('gulp-dotnet-assembly-info');
var rename = require('gulp-rename');
var msbuild = require('gulp-msbuild');
var nuget = require('gulp-nuget');
var bump = require('gulp-bump');
var xunit = require('gulp-xunit-runner');

//optional, lets you do .pipe(debug()) to see whats going on
var debug = require("gulp-debug");

var project = JSON.parse(fs.readFileSync("./package.json"));

var config = {
    name: project.name,
    version: project.version,
    mode: args.mode || "Debug",
    output: ".build/deploy",
    deployTarget: args.deployTarget,
    buildNumber: args.BuildVersion || "000",
    releasenotesfile: "ReleaseNotes.md"
}

gulp.task('default', ["restore", "version", "compile", "test"]);

gulp.task('restore', shell.task([
    '".build/tools/nuget.exe" restore -configFile nuget.config'
]));

gulp.task('version', function () {
    return gulp
        .src('.build/AssemblyVersion.base')
        .pipe(rename("AssemblyVersion.cs"))
        .pipe(assemblyInfo({
            version: config.version,
            fileVersion: config.version,
            description: "Build: " + config.buildNumber
        }))
        .pipe(gulp.dest(config.name + '/Properties'));
});

gulp.task('compile', ["restore", "version"], function () {
    return gulp
        .src(config.name + ".sln")
        .pipe(msbuild({
            targets: ["Clean", "Rebuild"],
            configuration: config.mode,
            toolsVersion: 14.0,
            errorOnFail: true,
            stdout: true,
            verbosity: "minimal",
            architecture: 'x64',
            nodeReuse: false,
            nologo: false,
            properties: {
                RunOctoPack: config.pack,
                VisualStudioVersion: 14.0,
            }
        }));
});

gulp.task('test', ["compile"], function () {
    return gulp
        .src(['**/bin/*/*.Tests.dll'], { read: false })
        .pipe(xunit({
            executable: 'packages/xunit.runner.console.2.2.0/tools/xunit.console.exe',
            options: {
                nologo: true,
                verbose: true,
            }
        }));
});

gulp.task('package', ["test"], shell.task([
    ' ".build/tools/nuget.exe"' +
    ' pack ' + config.name + '/' + config.name + '.csproj' +
    ' -version ' + config.version +
    ' -Prop Configuration=' + config.mode +
    ' -OutputDirectory ' + config.output
]));

gulp.task('deploy', ["package"], function () {

    return gulp
        .src(config.output + '/*.' + config.version + '.nupkg')
        .pipe(debug())
        .pipe(nuget.push({
            source: 'http://srv-bld-1:9999/nuget',
            nuget: ".build/tools/nuget.exe",
            apiKey: 'ThisIsAReallyStrongPassword123'
        }));
});

gulp.task('bump:patch', function () {
    return gulp
        .src("./package.json")
        .pipe(bump({ type: "patch" }))
        .pipe(gulp.dest('./'));
});

gulp.task('bump:minor', function () {
    return gulp
        .src("./package.json")
        .pipe(bump({ type: "minor" }))
        .pipe(gulp.dest('./'));
});

gulp.task('bump:major', function () {
    return gulp
        .src("./package.json")
        .pipe(bump({ type: "major" }))
        .pipe(gulp.dest('./'));
});
