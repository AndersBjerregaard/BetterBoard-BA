gh api graphql -f query='
	query{
		user(login: "AndersBjerregaard"){
			projectV2(number: 3) {
				id
			}
		}
	}'