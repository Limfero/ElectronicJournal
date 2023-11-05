import { Modal, Button } from "bootstrap";
import { useState } from "react"

const ModalButton = ({ modalContent, title, bthName}) => {
    const [showModal, setShowModal] = useState(false);

    const handleCloseModal = () => {
        setShowModal(false);
    };

    const handleOpenModal = () => {
        setShowModal(true);
    };

    return(
        <div>
            <Button variant="primary" onClick={handleOpenModal}>
                {bthName}
            </Button>

            <Modal show={showModal} onHide={handleCloseModal}>
                <Modal.Header closeButton>
                    <Modal.Title>{title}</Modal.Title>
                </Modal.Header>
                <Modal.Body>{modalContent}</Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleCloseModal}>
                    Закрыть
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    )
};

export default ModalButton;