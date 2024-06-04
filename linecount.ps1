# Counts the amount of lines of code in the repo
$files = git ls-files
$totalLines = 0
foreach ($file in $files) {
    Write-Output($file.ToString())
    $lineCount = (Get-Content $file | Measure-Object -Line).Lines
    $totalLines += $lineCount
}
$totalLines
