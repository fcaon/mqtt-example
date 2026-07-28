# Exemplo de subscrição em broker MQTT com dotnet

Este é pequeno projeto que visa apresentar uma forma de subscrição em um broker MQTT.

Este projeto também apresenta uma forma de subscrição em múltiplos tópicos em broker MQTT.

Os dados recebidos no(s) tópicos em que está subscrito, são apressentados no console apenas, mas é possível adaptar para que os mesmos sejam enviados para banco de dados.

# Tecnologia

Atualmente este mini projeto utiliza o .NetCore 9 como tecnologia de execução.

O sistema operacional utilizado como base para o desenvolvimento foi Linux, portanto, tudo deve funcionar igualmente em outros sistemas operacionais, porém é necessário um teste básico.

Este mesmo fonte também pode ser adaptado para funcionar como serviço no linux (já testado e utilizado em projeto real).

# Configurações

Sempre que necessário alterar tópicos ou endereço do broker MQTT, basta acessar o arquivo "appsettings.json".

O(s) tópico(s) deve(m) ser informado(s) em um array de string, onde cada posição do array será um tópico. IMPORTANTE: mesmo seja um único tópico, este deverá estar em um array de uma única posição.
