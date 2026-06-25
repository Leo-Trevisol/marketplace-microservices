document.addEventListener('DOMContentLoaded', function () {
    const estrelas = document.querySelectorAll('.avaliacao-estrelas .estrela');

    function pintarEstrelas(valorSelecionado) {
        estrelas.forEach(estrela => {
            const valor = parseInt(estrela.dataset.valor);
            if (valor <= valorSelecionado) {
                estrela.classList.remove('bi-star');
                estrela.classList.add('bi-star-fill', 'estrela-ativa');
            } else {
                estrela.classList.remove('bi-star-fill', 'estrela-ativa');
                estrela.classList.add('bi-star');
            }
        });
    }

    estrelas.forEach(estrela => {
        estrela.addEventListener('click', () => {
            pintarEstrelas(parseInt(estrela.dataset.valor));
        });

        estrela.addEventListener('mouseenter', () => {
            pintarEstrelas(parseInt(estrela.dataset.valor));
        });
    });

    document.querySelector('.avaliacao-estrelas')?.addEventListener('mouseleave', () => {
        const notaSelecionada = document.querySelector('.avaliacao-estrelas input[type=radio]:checked');
        pintarEstrelas(notaSelecionada ? parseInt(notaSelecionada.value) : 0);
    });

    const formAvaliacao = document.getElementById('formAvaliacao');

    if (formAvaliacao) {
        formAvaliacao.addEventListener('submit', async function (e) {
            e.preventDefault();

            const formData = new FormData(formAvaliacao);
            const submitBtn = formAvaliacao.querySelector('button[type="submit"]');
            const textoOriginal = submitBtn.innerHTML;

            submitBtn.disabled = true;
            submitBtn.innerHTML = 'Enviando...';

            try {
                const resp = await fetch(formAvaliacao.action, {
                    method: 'POST',
                    body: formData
                });

                const data = await resp.json();

                exibirToast(data.message, data.sucesso);

                if (data.sucesso) {
                    setTimeout(() => window.location.reload(), 1500);
                }
            } catch (err) {
                exibirToast('Erro ao enviar avaliação. Tente novamente!', false);
            } finally {
                submitBtn.disabled = false;
                submitBtn.innerHTML = textoOriginal;
            }
        });
    }

    function exibirToast(mensagem, sucesso) {
        const toastEl = document.getElementById('toastAvaliacao');
        const toastBody = document.getElementById('toastAvaliacaoBody');

        toastBody.textContent = mensagem;
        toastEl.classList.remove('bg-success', 'bg-danger');
        toastEl.classList.add(sucesso ? 'bg-success' : 'bg-danger');

        bootstrap.Toast.getOrCreateInstance(toastEl).show();
    }
});