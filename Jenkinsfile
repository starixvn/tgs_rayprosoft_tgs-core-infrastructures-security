pipeline {
	agent any
	
	environment {
		BuildConfiguration="Debug"
	}

	stages {
		stage('Build') {
			steps {
				sh 'dotnet build -c $BuildConfiguration'
			}
		}
		
		stage('Test') {
			steps {
				sh 'dotnet test -c $BuildConfiguration --logger trx --no-build'
			}
		}
		
		stage('Pack & Push packages') {
			steps {
				sh 'dotnet pack Tgs.Core.Infrastructures.Security/Tgs.Core.Infrastructures.Security.csproj -c Debug --no-build --no-restore'
				sh 'dotnet nuget push Tgs.Core.Infrastructures.Security/bin/$BuildConfiguration/*.nupkg -s "raypro-baget" -k admin --skip-duplicate'
			}
		}
	}
	
	 post {
        always {
            step([$class: 'MSTestPublisher', testResultsFile:"**/*.trx", failOnError: false, keepLongStdio: true])
        }
    }
}
