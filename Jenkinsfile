pipeline {
    agent any
	environment {
        CommitMsg = ""
        CommitPerson = ""
    }
    stages {
		stage ('取得新版(git)') {
            steps {
				git branch: 'master', credentialsId: '700a1e8e-746b-4dbe-b75c-d84eface67bb', url: 'https://github.com/LeoAmadeGX/FoodsReview.git'
		
				script {			
					def changeSets = currentBuild.rawBuild.changeSets
					def commitMsgList = []
					def commitPersonList = []
					
					if (changeSets) {
						for (changeSet in changeSets) {
							for (entry in changeSet.items) {
								// 檢查重複人員
								if (!commitMsgList.contains(entry.msg)) {
									commitMsgList.add(entry.msg)
								}
								commitPersonList.add(entry.author.fullName)
							}
						}
					} else {
						commitMsgList.add("No Commit Message found in this build.")
						commitPersonList.add("RDAdministrator")
					}
					CommitMsg = commitMsgList.join('\n ')
					CommitPerson = commitPersonList.join('\n ')
					
					echo "${CommitPerson} Commit ${CommitMsg}"
				}
			}
		}
		stage ('開始建置(指定位置)') {
            steps {
				bat '"C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin\\amd64\\MSBuild.exe" "C:\\sampleTest\\Systemweb.WFP.API.AdminAPI\\Systemweb.WFP.API.AdminAPI.csproj" /p:VisualStudioVersion=15.0 /t:Restore /t:rebuild /p:PackageOutput=false /p:Configuration=Release /p:DeployOnBuild=true;PublishProfile=FolderProfile.pubxml'
			}
		}
		stage ('備份&部署') {
            steps {
				echo "先不部署"
			}
		}		
		stage ('執行SysLogin') {
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
							error("HTTP Error ${response}: 请求失败")
						}
					} catch (Exception e) {
						currentBuild.result = 'FAILURE'
						error("调用 API 时发生错误: ${e.message}")
					}
				}
			}
		}
    }
	post {
		failure{
		    
			emailext to: "Leo_Tsai@systemweb.com.tw",
				subject: "jenkins RDAP2019 FoodsReviews 由 ${CommitPerson} 推送的 ${CommitMsg} 發版失敗\n\n已緊急使用昨日備份版本還原，請相關人員盡速處理",
				body: "${env.JOB_NAME} : ${currentBuild.currentResult}\n\n更多資訊請由此查詢: ${env.BUILD_URL}"
		}
		success {
			emailext to: "Leo_Tsai@systemweb.com.tw",
				subject: "jenkins RDAP2019 FoodsReviews 新版本已發布",
				body: "${env.JOB_NAME} : ${currentBuild.currentResult}\n\nRDAP2019 FoodsReviews 因 master 上由以下人員 : \n${CommitPerson} \n推送的以下commit : \n${CommitMsg} \n\n\n現在已更新到最新版本，請確認。\n\n更多資訊請由此查詢: ${env.BUILD_URL}"
		}
	}
}