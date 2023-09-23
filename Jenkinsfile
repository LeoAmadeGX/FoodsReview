pipeline {
    agent any
	environment {
        CommitMsg = ""
        CommitPerson = ""
		CommitNote = ""
		PublishSys = "FoodsReviews"
    }
    stages {
		stage ('���o�s��(git)') {
            steps {
				git branch: 'master', credentialsId: '700a1e8e-746b-4dbe-b75c-d84eface67bb', url: 'https://github.com/LeoAmadeGX/FoodsReview.git'
				
				script {			
					def changeSets = currentBuild.rawBuild.changeSets
					def uniqueAuthors = new HashSet<String>()
					def commitMsgList = []
					def commitPersonList = []
					def commitNoteList = []
					
					if (changeSets) {
						for (changeSet in changeSets) {
							for (entry in changeSet.items) {
								// �ˬd���ƤH��
								if (entry.author && !uniqueAuthors.contains(entry.author.fullName)) {
								    uniqueAuthors.add(entry.author.fullName)
								    commitPersonList.add(entry.author.fullName)
								}
								commitMsgList.add(entry.msg)
								commitNoteList.add(entry.author.fullName + ": " + entry.msg)
							}
						}
					} else {
						commitMsgList.add("Manually Execute Build Process.")
						commitPersonList.add("RDAdministrator")
						commitNoteList.add("RDAdministrator: Manually Execute Build Process.")
					}
					CommitMsg = commitMsgList.join('\n')
					CommitPerson = commitPersonList.join('\n')
					CommitNote = commitNoteList.join('\n')

					echo "${CommitPerson}"
					echo "${CommitMsg}"
					echo "${CommitNote}"
				}
			}
		}
		stage ('�}�l�ظm') {
            steps {
				bat '"C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin\\amd64\\MSBuild.exe" "FoodsReview.csproj" /p:VisualStudioVersion=15.0 /t:Restore /t:rebuild /p:PackageOutput=false /p:Configuration=Release /p:DeployOnBuild=true;PublishProfile=FolderProfile.pubxml'
			}
		}
		stage ('�ƥ�&���p') {
            steps {
				bat 'C:\\Jenkins\\workspace\\deploy.bat 1 FoodsReviews'
			}
		}		
		stage ('����SysLogin') {
			steps {
				script {
					try {
						// POST
						def post = new URL("http://localhost/AdminAPI/v2/api/SysLogin").openConnection();
						def message = '{"message":"I am Jenkins Test."}'
						post.setRequestMethod("POST")
						post.setDoOutput(true)
						post.setRequestProperty("Content-Type", "application/json")
						post.setRequestProperty("Accept-Language", "zh-TW")
						post.setRequestProperty("client_id", "WFPAPIPublicClient")
						post.getOutputStream().write(message.getBytes("UTF-8"));
						def postRC = post.getResponseCode();
						println(postRC);
						if(postRC.equals(200)) {
							println(post.getInputStream().getText());
						}
						else
						{
							currentBuild.result = 'FAILURE'
							error("HTTP Error ${response}: �ШD����")
						}
					} catch (Exception e) {
						currentBuild.result = 'FAILURE'
						error("�I�s API �ɵo�Ϳ��~: ${e.message}")
					}
				}
			}
		}
    }
	post {
		failure{
		    bat 'C:\\WebsiteBackup\\RestoreYesterDay.bat FoodsReviews'
		    
			emailext to: "Leo_Tsai@systemweb.com.tw",
				subject: "${PublishSys} �o������ on RDAP2019 #jenkins",
				body: "${env.JOB_NAME} : ${currentBuild.currentResult}\n\nRDAP2019 ${PublishSys} �]���H�UCommit : \n\n${CommitNote} \n�o������\n\n\n�w���ϥάQ��ƥ������٭�A�Ь����H���ɳt�B�z\n\n��h��T�ХѦ��d��: ${env.BUILD_URL}"
		}
		success {
			emailext to: "Leo_Tsai@systemweb.com.tw",
				subject: "${PublishSys} �s�����w�o�� on RDAP2019 #jenkins",
				body: "${env.JOB_NAME} : ${currentBuild.currentResult}\n\nRDAP2019 ${PublishSys} �] ReleasePatch �W�X�{�H�UCommit : \n\n${CommitNote} \n���e \n\n\n�{�b�w��s��̷s�����A�нT�{�C\n\n��h��T�ХѦ��d��: ${env.BUILD_URL}"
		}
	}
}