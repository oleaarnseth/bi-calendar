$ErrorActionPreference = 'Stop'

docker build -f ".\Dockerfile" --force-rm -t minikube/bicalendar:debug "..\"

$podQueryResult = $(kubectl get pod -l io.kompose.service=bicalendar -o json | ConvertFrom-Json)

kubectl apply -f .\MinikubeManifest.yaml

if ($podQueryResult.items.Length -gt 0) {
    # Delete old pod
    kubectl delete pod $podQueryResult.items[0].metadata.name
}
