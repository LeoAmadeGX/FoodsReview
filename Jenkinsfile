pipeline {
    agent any    
	triggers {
        pollSCM('0 17 * * *') 
    }
	environment {
        CommitMsg = ""
        CommitPerson = ""
		CommitNote = ""
		PublishSys = "FoodsReviews"
    }
    stages {
		stage ('取得新版(git)') {
            steps {
				git branch: 'master', credentialsId: '700a1e8e-746b-4dbe-b75c-d84eface67bb', url: 'https://github.com/LeoAmadeGX/FoodsReview.git'
				
				script {			
					def changeSets = currentBuild.rawBuild.changeSets
					def uniqueAuthors = new HashSet<String>()
					def commitMsgList = []
					def commitPersonList = []
					def commitNoteList = []
					def commitTimeList = []
					
					if (changeSets) {
						for (changeSet in changeSets) {
							for (entry in changeSet.items) {
								// 檢查重複人員
								if (entry.author && !uniqueAuthors.contains(entry.author.fullName)) {
								    uniqueAuthors.add(entry.author.fullName)
								    commitPersonList.add(entry.author.fullName)
								}
								def commitTimestamp = entry.timestamp
                                def formattedTime = new Date(commitTimestamp).format('yyyy/MM/dd HH:mm:ss')
                                commitTimeList.add(formattedTime)

								commitMsgList.add(entry.msg)
								commitNoteList.add(entry.author.fullName + ": " + entry.msg + "  " + formattedTime)
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
		stage ('開始建置') {
            steps {
				bat '"C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin\\amd64\\MSBuild.exe" "FoodsReview.csproj" /p:VisualStudioVersion=15.0 /t:Restore /t:rebuild /p:PackageOutput=false /p:Configuration=Release /p:DeployOnBuild=true;PublishProfile=FolderProfile.pubxml'
			}
		}
		stage ('備份&部署') {
            steps {
				bat 'C:\\Jenkins\\workspace\\deploy.bat 1 FoodsReviews'
			}
		}		
		stage ('取得網站狀態') {
			steps {
				script {
					try {// GET
						def get = new URL("http://rdap2019/FoodsReviews/").openConnection();
						def getRC = get.getResponseCode();
						println(getRC);
						if (getRC.equals(200)) {
						    println(get.getInputStream().getText());
						}
						else
						{
							currentBuild.result = 'FAILURE'
							error("HTTP Error ${response}: 請求失敗")
						}
					} catch (Exception e) {
						currentBuild.result = 'FAILURE'
						error("呼叫 API 時發生錯誤: ${e.message}")
					}
				}
			}
		}
    }
	post {
		failure{
		    bat 'C:\\WebsiteBackup\\RestoreYesterDay.bat FoodsReviews'
		    
			emailext to: "Leo_Tsai@systemweb.com.tw",
				subject: "${PublishSys} 發版失敗 on RDAP2019 #jenkins",
				body: "${env.JOB_NAME} : ${currentBuild.currentResult}\n\nRDAP2019 ${PublishSys} 因為以下Commit : \n\n${CommitNote} \n發版失敗\n\n\n已緊急使用昨日備份版本還原，請相關人員盡速處理\n\n更多資訊請由此查詢: ${env.BUILD_URL}"
		}
		success {
			emailext to: "Leo_Tsai@systemweb.com.tw",
				subject: "${PublishSys} 新版本已發布 on RDAP2019 #jenkins",
				body: "${env.JOB_NAME} : ${currentBuild.currentResult}\n\nRDAP2019 ${PublishSys} 因 ReleasePatch 上出現以下Commit : \n\n${CommitNote} \n推送 \n\n\n現在已更新到最新版本，請確認。\n\n更多資訊請由此查詢: ${env.BUILD_URL}"
		}
	}
}